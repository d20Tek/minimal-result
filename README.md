# d20Tek Minimal.Result
A straight-forward implementation of the Result object pattern. Used to return a monad result that will either be the returned value or a set of errors.

## Introduction
This package provides an implementation of the Result Object design pattern, which allows methods to return a uniform Result object that: represents a value when the method succeeds, and a list of errors when the method fails.

Many of the concepts in this library were inspired by - [Error handling: Exception or Result? by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/)

Along with the base Result, Result<T>, and IResult implementations, we also have extensions packages to integrate with ASP.NET and in application layer packages, like Mediatr and FluentValidations.

The D20Tek.Minimal.Result.AspNetCore package provides extension methods to convert our Result objects to either MinimalApi results or controller-based WebApi action results. There's a customizable mapping for how those are handled. And they can either be called in your API code or through middleware.

And for developers building Clean Architecture, we have the D20Tek.Minimal.Result.Extensions package. This package provides:
* An extension method to convert FluentValidations failure to our Result.Errors.
* An IPipelineBehavior implementation that integrates with Mediatr and performs validations on incoming messages. This automatically runs the validation before calling the message handler, so validation can be encapsulated outside of the handler logic.

There are also a full set of samples that use these packages in various WebApi scenarios.

## Installation
These libraries are NuGet packages so it is easy to add to your project. To install the packages into your solution, you can use the NuGet Package Manager. In PM, please use the following command:
```  
PM > Install-Package D20Tek.Minimal.Result -Version 1.0.1
PM > Install-Package D20Tek.Minimal.Result.AspNetCore -Version 1.0.1
PM > Install-Package D20Tek.Minimal.Result.Extensions -Version 1.0.1
``` 

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.Minimal.Result, and install whichever packages you require from there.

Note: This package is still in pre-release because I'm still ensuring that the API works cleanly. Once it's been used in several projects and the API solidifies, we will move it to a stable release.

## Usage
Once you've installed the NuGet package, you can start using it in your .NET projects.

### Implement method that returns Result
In the /samples/Samples.Application class library, you can find several queries and commands that return a Result. The following is the code for a create command:
```csharp
using D20Tek.Minimal.Result;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Members.Commands.CreateMember;

public sealed class CreateMemberCommandHandler
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        CreateMemberCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!await _memberRepository.IsEmailUniqueAsync(command.Email, cancellationToken))
        {
            return DomainErrors.Member.EmailAlreadyInUse;
        }

        var member = new Member(
            Guid.NewGuid(),
            command.FirstName,
            command.LastName,
            command.Email);

        await _memberRepository.AddAsync(member, cancellationToken);

        return member;
    }
}
```
Now, this method will either return the created Member object (when it succeeds) or an error (when there are duplicate emails found in the system). In a full implementation, we may also want to catch system exceptions and return them a Result.

Notice that we don't actually create a new Result<Member> with the value or errors in our code. The Result class implements some implicit conversion operators. 
* So by passing it a list of errors, the library will implicitly create a Result<Member> with IsFailure = true and the errors list.
* By passing an instance of the expected type, the library will implicitly create a Result<Member> with IsSuccess = true and the Value object specified.

### Code that handles the Result
For the simplest view on how the result is handled, let's review the CreateNewMember method in the /samples/Basic.Cli project. 

```csharp
static async Task<Guid> CreateNewMember(IMemberRepository memberRepository)
{
    Console.Write("Creating new member... ");
    var createHandler = new CreateMemberCommandHandler(memberRepository);
    var createdResult = await createHandler.Handle(new CreateMemberCommand("Foo", "Bar", "foo@bar.com"));

    return createdResult.IfOrElse(
        success =>
        {
            Console.WriteLine("member created successfully!");
            return success.Id;
        },
        error =>
        {
            Console.WriteLine("member creation failed.");
            Console.WriteLine(error.First().ToString());
            return Guid.Empty;
        });
}
```

The CreateCommandHandler returns its Result. We then use that result, in the Match and MatchFirstError method to perform specific functions one a success or error state. 
* When the result succeeds, we call the first function with success as the input parameter (it's the instance of the object in the result).
* When the reuslt fails, we call the second function with the first error in the Result.Errors list. There is also a similar Match method, that will provide the full Errors list as input.
* These functions are done a simple anonymous functions to simplify the code. But users can also call the Result.IsSuccess and Result.IsFailure properties to build that logic.

### ASP.NET Code that handles the Result
Now the command caller can manage its control flow based on this result. In the /samples/Basic.MinimalApi, the MembersEndpoint.cs does just that. Here is a code snippet that handles the result of calling the CreateMemberCommandHandler: 

```csharp
using Basic.MinimalApi.Contracts;
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Mvc;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Core.Entities;

namespace Basic.MinimalApi.Endpoints;

public static class MembersEndpoint
{
    public static void MapMemberEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/members").WithTags("Members");

        ...
        group.MapPost("/", async (
            [FromBody] CreateMemberRequest request,
            [FromServices] CreateMemberCommandHandler commandHandler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateMemberCommand(
                request.FirstName,
                request.LastName,
                request.Email);
            var result = await commandHandler.Handle(command, cancellationToken);

            var routeValues = result.IsSuccess ? new { id = result.Value!.Id } : null;
            return result.ToCreatedApiResult(ToResponse, "GetMemberById", routeValues);
        })
        .WithName("CreateMember")
        .Produces<MemberResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithOpenApi();
        ...
    }
}
```

This code uses the D20Tek.Minimal.Result.AspNetCore package, the result.ToCreatedApiResult converts the Result to the appropriate TypeResults or ProblemDetails (for any errors). Within the AspNetCore extension methods, we're call Result.Match to process the success and error paths.

### Samples:
For more detailed examples on how to use D20Tek.Minimal.Result, please review the following samples:

* [Samples.Core](samples/Samples.Core) - Core domain project shared by all of the Api projects (Domain layer in Clean Architecture).
* [Samples.Infrastructure](samples/Samples.Infrastructure) - Infrastructure project that implements the in-memory MemberRepository.
* [Samples.Application](samples/Samples.Application) - Application project that implements use-cases for our Api projects.
* [Basic.Cli](samples/Basic.Cli) - Simple console app that uses the various Clean Architecture layers to save and query Member data. Example of directly consuming and handling the Result object.
* [Basic.MinimalApi](samples/Basic.MinimalApi) - Minimal Api implementation that consumes the Result object from the Application. And uses the D20Tek.Minimal.Result.AspNetCore package to convert Result object to TypedResults or ProblemDetails.
* [Basic.WebApi](samples/BasicWebApi) - Controller-based WebApi implementation that consumes the Result object from the Application. And uses the D20Tek.Minimal.Result.AspNetCore package to convert Result object to ActionResult/IActionResult or ProblemDetails.
* [Samples.Application.Mediatr](samples/Samples.Application.Mediatr) - Application project that uses Mediatr and FluentValidations to implement use-cases for our Api projects.
* [Full.MinimalApi](samples/Full.MinimalApi) - Minimal Api implementation that consumes the Result object from the Application. And uses the D20Tek.Minimal.Result.Extensions package to integrate with FluentValidations and Mediatr in a full Clean Architecture implementation.


## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository. I'm still in the process of building these libraries, so any suggestions that would make it more useable are welcome.

