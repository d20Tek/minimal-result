//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Samples.Application.Members.Commands.CreateMember;

public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email);
