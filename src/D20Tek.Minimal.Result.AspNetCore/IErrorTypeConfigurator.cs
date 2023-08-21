//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Net;

namespace D20Tek.Patterns.Result.AspNetCore
{
    public interface IErrorTypeConfigurator
    {
        IErrorTypeConfigurator Clear();

        IErrorTypeConfigurator For(int errorType, HttpStatusCode statusCode);

        IErrorTypeConfigurator Remove(int errorType);
    }
}