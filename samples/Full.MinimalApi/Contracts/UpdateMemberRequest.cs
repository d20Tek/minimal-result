//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Full.MinimalApi.Contracts;

public sealed record UpdateMemberRequest(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
