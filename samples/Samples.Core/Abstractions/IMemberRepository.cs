//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Samples.Core.Entities;

namespace Samples.Core.Abstractions;

public interface IMemberRepository
{
    public Task<Member?> GetByIdAsync(
        Guid memberId,
        CancellationToken cancellationToken = default);

    public Task<Member?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    public Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default);

    public Task AddAsync(
        Member member,
        CancellationToken cancellationToken = default);

    public Task UpdateAsync(
        Member member,
        CancellationToken cancellationToken = default);

    public Task DeleteAsync(
        Guid memberId,
        CancellationToken cancellationToken = default);
}
