//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Basic.WebApi.Contracts;
using D20Tek.Minimal.Result.AspNetCore.WebApi;
using Microsoft.AspNetCore.Mvc;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Application.Members.Queries.GetMemberById;

namespace Basic.WebApi.Controllers;

[Route("api/v1/members")]
[ApiController]
public sealed class MembersController : ControllerBase
{
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByEmail")]
    public async Task<ActionResult<MemberResponse>> Get(
        [FromRoute] string email,
        [FromServices] GetMemberByEmailQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        var result = await queryHandler.Handle(
            new GetMemberByEmailQuery(email),
            cancellationToken);

        return result.ToActionResult(MemberMapper.Convert, this);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberById")]
    public async Task<ActionResult<MemberResponse>> Get(
        [FromRoute] Guid id,
        [FromServices] GetMemberByIdQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        var result = await queryHandler.Handle(new GetMemberByIdQuery(id), cancellationToken);
        return result.ToActionResult(MemberMapper.Convert, this);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("CreateMember")]
    public async Task<ActionResult<MemberResponse>> Post(
        [FromBody] CreateMemberRequest request,
        [FromServices] CreateMemberCommandHandler commandHandler,
        CancellationToken cancellationToken)
    {
        var command = new CreateMemberCommand(
            request.FirstName,
            request.LastName,
            request.Email);
        var result = await commandHandler.Handle(command, cancellationToken);

        var routeValues = result.IsSuccess ? new { id = result.Value!.Id } : null;
        return result.ToCreatedActionResult(
            MemberMapper.Convert,
            this,
            "GetMemberById",
            routeValues);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("UpdateMember")]
    public async Task<ActionResult<MemberResponse>> Put(
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
        return result.ToActionResult(MemberMapper.Convert, this);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("DeleteMember")]
    public async Task<ActionResult<MemberResponse>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteMemberCommandHandler commandHandler,
        CancellationToken cancellationToken)
    {
        var result = await commandHandler.Handle(
            new DeleteMemberCommand(id),
            cancellationToken);

        return result.ToActionResult(MemberMapper.Convert, this);
    }
}
