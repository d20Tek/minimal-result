//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Patterns.Result.UnitTests;

[ExcludeFromCodeCoverage]
public sealed record TestResponse(int Id, string Message);

[ExcludeFromCodeCoverage]
public sealed record TestEntity(int Id, string Message, DateTime CreatedDate);

public sealed class TestController : ControllerBase
{
}
