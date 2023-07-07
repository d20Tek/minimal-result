//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Samples.Application.Members.Commands.UpdateMember;

public sealed record UpdateMemberCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
