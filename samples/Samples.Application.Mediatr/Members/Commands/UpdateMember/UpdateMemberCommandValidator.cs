//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentValidation;

namespace Samples.Application.Mediatr.Members.Commands.UpdateMember;

public sealed class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithErrorCode("Id.Empty");
        RuleFor(x => x.FirstName).NotEmpty().WithErrorCode("FirstName.Empty");
        RuleFor(x => x.LastName).NotEmpty().WithErrorCode("LastName.Empty");
        RuleFor(x => x.Email).NotEmpty().WithErrorCode("Email.Empty");
        RuleFor(x => x.Email).EmailAddress().WithErrorCode("Email.InvalidFormat");
    }
}
