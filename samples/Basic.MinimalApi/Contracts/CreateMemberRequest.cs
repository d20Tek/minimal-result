//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Basic.MinimalApi.Contracts;

public sealed record CreateMemberRequest(
    string FirstName,
    string LastName,
    string Email);
