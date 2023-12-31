﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

[ExcludeFromCodeCoverage]
public sealed record TestRequest(int Id) : IRequest<Result<TestResponse>>;

[ExcludeFromCodeCoverage]
public sealed record TestResponse(int Id, string Message);

[ExcludeFromCodeCoverage]
public sealed record TestEntity(int Id, string Message, DateTime CreatedDate);

public sealed class TestController : ControllerBase
{
}
