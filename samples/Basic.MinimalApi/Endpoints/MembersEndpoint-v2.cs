//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Basic.MinimalApi.Contracts;
using D20Tek.Patterns.Result.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Mvc;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Application.Members.Queries.GetMemberById;
using Samples.Core.Entities;

namespace Basic.MinimalApi.Endpoints;

public static class MembersEndpointV2
{
    public static void MapMemberEndpointsV2 (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/members")
                          .WithTags("Members V2")
                          .AddEndpointFilter<HandleTypedResultFilter<MemberResponse>>();
                          //.AddEndpointFilter<HandleResultFilter>();

        group.MapGet("/email/{email}", async (
            [FromRoute] string email,
            [FromServices] GetMemberByEmailQueryHandler queryHandler,
            CancellationToken cancellationToken) =>
        {
            var result = await queryHandler.Handle(
                new GetMemberByEmailQuery(email),
                cancellationToken);

            return result.MapResult(ToResponse);
        })
        .WithName("GetMemberByEmailV2")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApi();

        group.MapGet("/{id:Guid}", async (
            [FromRoute] Guid id,
            [FromServices] GetMemberByIdQueryHandler queryHandler,
            CancellationToken cancellationToken) =>
        {
            var result = await queryHandler.Handle(new GetMemberByIdQuery(id), cancellationToken);
            return result.MapResult(ToResponse);
        })
        .WithName("GetMemberByIdV2")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApi();

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

            return result.MapResult(ToResponse);
        })
        .WithName("CreateMemberV2")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .Produces<MemberResponse>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        group.MapPut("/{id:Guid}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateMemberRequest request,
            [FromServices] UpdateMemberCommandHandler commandHandler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateMemberCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Email);

            var result = await commandHandler.Handle(command, cancellationToken);
            return result.MapResult(ToResponse);
        })
        .WithName("UpdateMemberV2")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        group.MapDelete("/{id:Guid}", async (
            [FromRoute] Guid id,
            [FromServices] DeleteMemberCommandHandler commandHandler,
            CancellationToken cancellationToken) =>
        {
            var result = await commandHandler.Handle(
                new DeleteMemberCommand(id),
                cancellationToken);

            return result.MapResult(ToResponse);
        })
        .WithName("DeleteMemberV2")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApi();
    }

    private static MemberResponse ToResponse(Member member)
    {
        return new MemberResponse(
            member.Id,
            member.FirstName,
            member.LastName,
            member.Email);
    }
}
