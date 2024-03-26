//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

[ExcludeFromCodeCoverage]
public class Tester : IEquatable<Tester>
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public override string ToString()
    {
        return $"{Name} [{Id}]";
    }

    public override int GetHashCode()
    {
        return 42;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Tester other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(Tester? other) => Id == other?.Id;
}
