//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Basic.WebApi.Contracts;

public sealed record MemberResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
