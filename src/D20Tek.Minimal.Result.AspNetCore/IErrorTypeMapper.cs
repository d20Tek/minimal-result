//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Net;

namespace D20Tek.Patterns.Result.AspNetCore
{
    public interface IErrorTypeMapper
    {
        IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null);

        bool Contains(int errorType);
        
        HttpStatusCode Convert(int errorType);

        IErrorTypeMapper For(int errorType, HttpStatusCode statusCode);

        IErrorTypeMapper Remove(int errorType);
    }
}