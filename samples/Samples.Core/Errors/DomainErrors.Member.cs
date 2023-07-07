//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;

namespace Samples.Core.Errors;

public static partial class DomainErrors
{
    public static class Member
    {
        public static Error NotFound(Guid memberId) => Error.NotFound(
                "Member.NotFound",
                $"The member with Id {memberId} was not found.");

        public static Error NotFound(string email) => Error.NotFound(
                "Member.NotFound",
                $"The member with Email: {email} was not found.");

        public static readonly Error FirstNameEmpty = Error.Validation(
                "FirstName.Empty",
                "First name is empty.");

        public static readonly Error FirstNameTooLong = Error.Validation(
                "FirstName.TooLong",
                "First name is too long.");

        public static readonly Error LastNameEmpty = Error.Validation(
                "LastName.Empty",
                "LAst name is empty.");

        public static readonly Error LastNameTooLong = Error.Validation(
                "LastName.TooLong",
                "Last name is too long.");

        public static readonly Error EmailEmpty = Error.Validation(
                "Email.Empty",
                "Email is empty.");

        public static readonly Error EmailTooLong = Error.Validation(
                "Email.TooLong",
                "Email is too long.");

        public static readonly Error EmailInvalidFormat = Error.Validation(
                "Email.InvalidFormat",
                "Email property is not a valid email format.");

        public static readonly Error EmailAlreadyInUse = Error.Conflict(
                "Email.AlreadyInUse",
                "Email address is already in use and must be unique per member.");
    }
}
