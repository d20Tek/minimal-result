//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using MediatR;
using Samples.Core.Entities;

namespace Samples.Application.Mediatr.Members.Commands.CreateMember;

public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email) : IRequest<Result<Member>>;
