//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Basic.WebApi.Contracts;

public sealed record CreateMemberRequest(
    string FirstName,
    string LastName,
    string Email);
