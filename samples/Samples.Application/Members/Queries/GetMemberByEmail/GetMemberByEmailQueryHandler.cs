﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Members.Queries.GetMemberByEmail;

public sealed class GetMemberByEmailQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByEmailQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        GetMemberByEmailQuery query,
        CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByEmailAsync(query.Email, cancellationToken);
        if (member is null)
        {
            return DomainErrors.Member.NotFound(query.Email);
        }

        return Result<Member>.Success(member);
    }
}
