//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;
using MediatR;
using Samples.Core.Entities;

namespace Samples.Application.Mediatr.Members.Queries.GetMemberById;

public sealed record GetMemberByIdQuery(Guid Id) : IRequest<Result<Member>>;