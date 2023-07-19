//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace Samples.Application.Mediatr.Members.Commands.CreateMember;

public sealed class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithErrorCode("FirstName.Empty");
        RuleFor(x => x.LastName).NotEmpty().WithErrorCode("LastName.Empty");
        RuleFor(x => x.Email).NotEmpty().WithErrorCode("Email.Empty");
        RuleFor(x => x.Email).EmailAddress().WithErrorCode("Email.InvalidFormat");
    }
}
