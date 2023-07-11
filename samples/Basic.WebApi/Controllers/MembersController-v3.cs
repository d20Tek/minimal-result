﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Basic.WebApi.Contracts;
using D20Tek.Patterns.Result;
using Microsoft.AspNetCore.Mvc;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Application.Members.Queries.GetMemberById;
using Samples.Core.Entities;

namespace Basic.WebApi.Controllers;

[Route("api/v3/members")]
[ApiController]
public class MembersControllerV3 : ControllerBase
{
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByEmailv3")]
    public async Task<Result<Member>> Get(
        [FromRoute] string email,
        [FromServices] GetMemberByEmailQueryHandler queryHandler)
    {
        return await queryHandler.Handle(new GetMemberByEmailQuery(email));
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByIdv3")]
    public async Task<Result<Member>> Get(
        [FromRoute] Guid id,
        [FromServices] GetMemberByIdQueryHandler queryHandler)
    {
        return await queryHandler.Handle(new GetMemberByIdQuery(id));
    }

    [HttpPost]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("CreateMemberv3")]
    public async Task<Result<Member>> Post(
        [FromBody] CreateMemberRequest request,
        [FromServices] CreateMemberCommandHandler commandHandler)
    {
        var command = new CreateMemberCommand(
            request.FirstName,
            request.LastName,
            request.Email);
        return await commandHandler.Handle(command);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("UpdateMemberv3")]
    public async Task<Result<Member>> Put(
        [FromRoute] Guid id,
        [FromBody] UpdateMemberRequest request,
        [FromServices] UpdateMemberCommandHandler commandHandler)
    {
        var command = new UpdateMemberCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email);

        return await commandHandler.Handle(command);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("DeleteMemberv3")]
    public async Task<Result<Member>> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteMemberCommandHandler commandHandler)
    {
        return await commandHandler.Handle(new DeleteMemberCommand(id));
    }
}