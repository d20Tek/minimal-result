//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using MediatR;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Mediatr.Members.Commands.UpdateMember;

internal sealed class UpdateMemberCommandHandler :
    IRequestHandler<UpdateMemberCommand, Result<Member>>
{
    private readonly IMemberRepository _memberRepository;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        UpdateMemberCommand command,
        CancellationToken cancellationToken)
    {
        var existingMember = await _memberRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingMember is null)
        {
            return DomainErrors.Member.NotFound(command.Id);
        }

        var member = new Member(
            command.Id,
            command.FirstName,
            command.LastName,
            command.Email);

        await _memberRepository.UpdateAsync(member, cancellationToken);

        return member;
    }
}
