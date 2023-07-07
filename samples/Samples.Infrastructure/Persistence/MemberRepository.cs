//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Samples.Core.Abstractions;
using Samples.Core.Entities;

namespace Samples.Infrastructure.Persistence;

public sealed class MemberRepository : IMemberRepository
{
    private static readonly Dictionary<Guid, Member> _members = new();

    public Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var member = _members.Values.FirstOrDefault(p => p.Email == email);
        return Task.FromResult(member);
    }

    public Task<Member?> GetByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        _members.TryGetValue(memberId, out Member? member);
        return Task.FromResult(member);
    }

    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
    {
        var members = _members.Values.Where(x => x.Email == email);
        return Task.FromResult(!members.Any());
    }

    public Task AddAsync(Member member, CancellationToken cancellationToken)
    {
        _members.Add(member.Id, member);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Member member, CancellationToken cancellationToken)
    {
        _members[member.Id] = member;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid memberId, CancellationToken cancellationToken)
    {
        _members.Remove(memberId);
        return Task.CompletedTask;
    }
}
