//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using MediatR;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Mediatr.Members.Commands.DeleteMember;

internal sealed class DeleteMemberCommandHandler :
    IRequestHandler<DeleteMemberCommand, Result<Member>>
{
    private readonly IMemberRepository _memberRepository;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        DeleteMemberCommand command,
        CancellationToken cancellationToken)
    {
        var existingMember = await _memberRepository.GetByIdAsync(
            command.MemberId,
            cancellationToken);

        if (existingMember is null)
        {
            return DomainErrors.Member.NotFound(command.MemberId);
        }

        await _memberRepository.DeleteAsync(existingMember.Id, cancellationToken);

        return existingMember;
    }
}
