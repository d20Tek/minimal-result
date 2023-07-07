﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Members.Commands.CreateMember;

public sealed class CreateMemberCommandHandler
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        CreateMemberCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!await _memberRepository.IsEmailUniqueAsync(command.Email, cancellationToken))
        {
            return DomainErrors.Member.EmailAlreadyInUse;
        }

        var member = new Member(
            Guid.NewGuid(),
            command.FirstName,
            command.LastName,
            command.Email);

        await _memberRepository.AddAsync(member, cancellationToken);

        return member;
    }
}