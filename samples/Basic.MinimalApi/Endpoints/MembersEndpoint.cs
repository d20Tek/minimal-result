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

public static class MembersEndpoint
{
    public static void MapMemberEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v1/members").WithTags("Members");

        group.MapGet("/email/{email}", async (
            [FromRoute] string email,
            [FromServices] GetMemberByEmailQueryHandler queryHandler) =>
        {
            var result = await queryHandler.Handle(new GetMemberByEmailQuery(email));
            return result.ToApiResult(ToResponse);
        })
        .WithName("GetMemberByEmail")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApi();

        group.MapGet("/{id:Guid}", async (
            [FromRoute] Guid id,
            [FromServices] GetMemberByIdQueryHandler queryHandler) =>
        {
            var result = await queryHandler.Handle(new GetMemberByIdQuery(id));
            return result.ToApiResult(ToResponse);
        })
        .WithName("GetMemberById")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApi();

        group.MapPost("/", async (
            [FromBody] CreateMemberRequest request,
            [FromServices] CreateMemberCommandHandler commandHandler) =>
        {
            var command = new CreateMemberCommand(
                request.FirstName,
                request.LastName,
                request.Email);
            var result = await commandHandler.Handle(command);

            var routeValues = result.IsSuccess ? new { id = result.Value.Id } : null;
            return result.ToCreatedApiResult(ToResponse, "GetMemberById", routeValues);
        })
        .WithName("CreateMember")
        .Produces<MemberResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        group.MapPut("/{id:Guid}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateMemberRequest request,
            [FromServices] UpdateMemberCommandHandler commandHandler) =>
        {
            var command = new UpdateMemberCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Email);

            var result = await commandHandler.Handle(command);
            return result.ToApiResult(ToResponse);
        })
        .WithName("UpdateMember")
        .Produces<MemberResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithOpenApi();

        group.MapDelete("/{id:Guid}", async (
            [FromRoute] Guid id,
            [FromServices] DeleteMemberCommandHandler commandHandler) =>
        {
            var result = await commandHandler.Handle(new DeleteMemberCommand(id));
            return result.ToApiResult(ToResponse);
        })
        .WithName("DeleteMember")
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
