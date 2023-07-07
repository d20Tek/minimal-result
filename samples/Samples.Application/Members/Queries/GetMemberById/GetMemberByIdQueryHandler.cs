//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Members.Queries.GetMemberById;

public sealed class GetMemberByIdQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        GetMemberByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(query.Id, cancellationToken);
        if (member is null)
        {
            return DomainErrors.Member.NotFound(query.Id);
        }

        return Result<Member>.Success(member);
    }
}
