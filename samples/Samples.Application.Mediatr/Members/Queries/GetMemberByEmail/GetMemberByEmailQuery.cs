//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;
using MediatR;
using Samples.Core.Entities;

namespace Samples.Application.Mediatr.Members.Queries.GetMemberByEmail;

public sealed record GetMemberByEmailQuery(string Email) : IRequest<Result<Member>>;