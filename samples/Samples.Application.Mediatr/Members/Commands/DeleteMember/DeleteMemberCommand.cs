//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using MediatR;
using Samples.Core.Entities;

namespace Samples.Application.Mediatr.Members.Commands.DeleteMember;

public sealed record DeleteMemberCommand(Guid MemberId) : IRequest<Result<Member>>;
