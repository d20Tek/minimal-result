//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Basic.WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Samples.Application.Members.Commands.CreateMember;
using Samples.Application.Members.Commands.DeleteMember;
using Samples.Application.Members.Commands.UpdateMember;
using Samples.Application.Members.Queries.GetMemberByEmail;
using Samples.Application.Members.Queries.GetMemberById;

namespace Basic.WebApi.Controllers
{
    [Route("api/v1/members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        [HttpGet("/email/{email}")]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string Get(
            [FromRoute] string email,
            [FromServices] GetMemberByEmailQueryHandler queryHandler)
        {
            return "value-email";
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string Get(
            [FromRoute] Guid id,
            [FromServices] GetMemberByIdQueryHandler queryHandler)
        {
            return "value";
        }

        [HttpPost]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Post(
            [FromBody] CreateMemberRequest request,
            [FromServices] CreateMemberCommandHandler commandHandler)
        {
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Put(
            [FromRoute] Guid id,
            [FromBody] UpdateMemberRequest request,
            [FromServices] UpdateMemberCommandHandler commandHandler)
        {
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public void Delete(
            [FromRoute] Guid id,
            [FromServices] DeleteMemberCommandHandler commandHandler)
        {
        }
    }
}
