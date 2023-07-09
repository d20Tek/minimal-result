//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Basic.WebApi.Contracts;

public sealed record UpdateMemberRequest(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
