//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Basic.WebApi.Contracts;
using D20Tek.Patterns.Result;
using D20Tek.Patterns.Result.AspNetCore.WebApi;
using Microsoft.AspNetCore.Mvc;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Application.Members.Queries.GetMemberById;

namespace Basic.WebApi.Controllers;

[Route("api/v3/members")]
[ApiController]
[ServiceFilter(typeof(HandleResultFilter))]
public sealed class MembersControllerV3 : ControllerBase
{
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByEmailv3")]
    public async Task<Result<MemberResponse>> Get(
        [FromRoute] string email,
        [FromServices] GetMemberByEmailQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        var result = await queryHandler.Handle(
            new GetMemberByEmailQuery(email),
            cancellationToken);

        return result.MapResult(MemberMapper.Convert);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByIdv3")]
    public async Task<Result<MemberResponse>> Get(
        [FromRoute] Guid id,
        [FromServices] GetMemberByIdQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        var result = await queryHandler.Handle(new GetMemberByIdQuery(id), cancellationToken);
        return result.MapResult(MemberMapper.Convert);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("CreateMemberv3")]
    public async Task<Result<MemberResponse>> Post(
        [FromBody] CreateMemberRequest request,
        [FromServices] CreateMemberCommandHandler commandHandler,
        CancellationToken cancellationToken)
    {
        var command = new CreateMemberCommand(
            request.FirstName,
            request.LastName,
            request.Email);
        var result = await commandHandler.Handle(command, cancellationToken);
        return result.MapResult(MemberMapper.Convert);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("UpdateMemberv3")]
    public async Task<Result<MemberResponse>> Put(
        [FromRoute] Guid id,
        [FromBody] UpdateMemberRequest request,
        [FromServices] UpdateMemberCommandHandler commandHandler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMemberCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email);

        var result = await commandHandler.Handle(command, cancellationToken);
        return result.MapResult(MemberMapper.Convert);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("DeleteMemberv3")]
    public async Task<Result<MemberResponse>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteMemberCommandHandler commandHandler,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(
            new DeleteMemberCommand(id),
            cancellationToken);

        return result.MapResult(MemberMapper.Convert);
    }
}
