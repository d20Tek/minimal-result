//---------------------------------------------------------------------------------------------------------------------
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

        var validationResult = ValidateCommand(command);
        if (validationResult.IsValid is false)
        {
            return validationResult.ToResult<Member>();
        }

        var member = new Member(
            Guid.NewGuid(),
            command.FirstName,
            command.LastName,
            command.Email);

        await _memberRepository.AddAsync(member, cancellationToken);

        return member;
    }

    private ValidationsResult ValidateCommand(CreateMemberCommand command)
    {
        var vResult = new ValidationsResult();

        if (string.IsNullOrWhiteSpace(command.FirstName))
        {
            vResult.AddValidationError(DomainErrors.Member.FirstNameEmpty);
        }

        if (string.IsNullOrWhiteSpace(command.LastName))
        {
            vResult.AddValidationError(DomainErrors.Member.LastNameEmpty);
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            vResult.AddValidationError(DomainErrors.Member.EmailEmpty);
        }

        if (command.Email.Split('@').Length != 2)
        {
            vResult.AddValidationError(DomainErrors.Member.EmailInvalidFormat);
        }

        return vResult;
    }
}
