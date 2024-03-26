//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using static D20Tek.Minimal.Result.UnitTests.OptionalTests;

namespace D20Tek.Minimal.Result.UnitTests;

[TestClass]
public class OptionalOverrideTests
{
    [TestMethod]
    public void ToString_WithValue_ResturnsString()
    {
        // arrange
        var t = new Tester { Id = "test-id", Name = "foo" };
        var v = Optional<Tester>.OfNullable(t);

        // act
        var result = v.ToString();

        // assert
        Assert.AreEqual("foo [test-id]", result);
    }

    [TestMethod]
    public void ToString_WithNull_ResturnsEmptyString()
    {
        // arrange
        var v = Optional<Tester>.Empty();

        // act
        var result = v.ToString();

        // assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void GetHashcode_WithValue_ResturnsString()
    {
        // arrange
        var t = new Tester { Id = "test-id", Name = "foo" };
        var v = Optional<Tester>.OfNullable(t);

        // act
        var result = v.GetHashCode();

        // assert
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void GetHashcode_WithNull_ResturnsZero()
    {
        // arrange
        var v = Optional<string>.Empty();

        // act
        var result = v.GetHashCode();

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_WithSameId_ResturnsTrue()
    {
        // arrange
        var t1 = new Tester { Id = "test-id", Name = "foo" };
        var t2 = new Tester { Id = "test-id", Name = "bar" };
        var op1 = Optional<Tester>.Of(t1);
        var op2 = Optional<Tester>.Of(t2);

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_WithDifferentId_ResturnsFalse()
    {
        // arrange
        var t1 = new Tester { Id = "test-id", Name = "foo" };
        var t2 = new Tester { Id = "test-id-2", Name = "foo" };
        var op1 = Optional<Tester>.Of(t1);
        var op2 = Optional<Tester>.Of(t2);

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithNullOther_ResturnsFalse()
    {
        // arrange
        var t1 = new Tester { Id = "test-id", Name = "foo" };
        var op1 = Optional<Tester>.Of(t1);
        Optional<Tester>? op2 = null;

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_WithEmptyValue_ResturnsFalse()
    {
        // arrange
        var t2 = new Tester { Id = "test-id-2", Name = "foo" };
        var op1 = Optional<Tester>.Empty();
        var op2 = Optional<Tester>.Of(t2);

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsObject_WithSameId_ResturnsTrue()
    {
        // arrange
        var t1 = new Tester { Id = "test-id", Name = "foo" };
        var t2 = new Tester { Id = "test-id", Name = "bar" };
        object op1 = Optional<Tester>.Of(t1);
        object op2 = Optional<Tester>.Of(t2);

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void EqualsObject_WithDifferentId_ResturnsFalse()
    {
        // arrange
        var t1 = new Tester { Id = "test-id", Name = "foo" };
        var t2 = new Tester { Id = "test-id-2", Name = "foo" };
        object op1 = Optional<Tester>.Of(t1);
        object op2 = Optional<Tester>.Of(t2);

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void EqualsObject_WithDifferentType_ResturnsFalse()
    {
        // arrange
        var t1 = new Tester { Id = "test-id", Name = "foo" };
        object op1 = Optional<Tester>.Of(t1);
        object op2 = "foo";

        // act
        var result = op1.Equals(op2);

        // assert
        Assert.IsFalse(result);
    }
}
