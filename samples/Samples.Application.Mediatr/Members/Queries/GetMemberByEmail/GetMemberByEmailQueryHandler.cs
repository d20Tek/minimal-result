//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;
using MediatR;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Mediatr.Members.Queries.GetMemberByEmail;

internal sealed class GetMemberByEmailQueryHandler :
    IRequestHandler<GetMemberByEmailQuery, Result<Member>>
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByEmailQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        GetMemberByEmailQuery query,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByEmailAsync(query.Email, cancellationToken);
        if (member is null)
        {
            return DomainErrors.Member.NotFound(query.Email);
        }

        return Result<Member>.Success(member);
    }
}
