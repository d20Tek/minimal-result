//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result;
using MediatR;
using Samples.Core.Abstractions;
using Samples.Core.Entities;
using Samples.Core.Errors;

namespace Samples.Application.Mediatr.Members.Queries.GetMemberById;

internal sealed class GetMemberByIdQueryHandler :
    IRequestHandler<GetMemberByIdQuery, Result<Member>>
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(
        GetMemberByIdQuery query,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(query.Id, cancellationToken);
        if (member is null)
        {
            return DomainErrors.Member.NotFound(query.Id);
        }

        return Result<Member>.Success(member);
    }
}
