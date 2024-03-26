//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

[TestClass]
public partial class OptionalTests
{
    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Value_WithNull_ThrowsException()
    {
        // arrange
        var opt = Optional<string>.Empty();

        // act
        _ = opt.Value;

        // assert
    }

    [TestMethod]
    public void Implicit_WithStringValue_CreatesOptional()
    {
        // arrange

        // act
        Optional<string> v = "implicit";

        // assert
        Assert.IsNotNull(v);
        Assert.IsTrue(v.HasValue);
        Assert.AreEqual("implicit", v.Value);
    }

    [TestMethod]
    public void Of_WithStringValue_CreatesOptional()
    {
        // arrange

        // act
        var v = Optional<string>.Of("test");

        // assert
        Assert.IsNotNull(v);
        Assert.IsTrue(v.HasValue);
        Assert.AreEqual("test", v.Value);
    }

    [TestMethod]
    public void OfNullable_WithValue_CreatesOptional()
    {
        // arrange
        var t = new Tester { Id = "test-id", Name = "foo" };

        // act
        var v = Optional<Tester>.OfNullable(t);

        // assert
        Assert.IsNotNull(v);
        Assert.IsTrue(v.HasValue);
        Assert.AreEqual(t, v.Value);
    }

    [TestMethod]
    public void OfNullable_WithNull_CreatesEmptyOptional()
    {
        // arrange

        // act
        var v = Optional<Tester>.OfNullable(null);

        // assert
        Assert.IsNotNull(v);
        Assert.IsFalse(v.HasValue);
    }

    [TestMethod]
    public void Empty_CreatesEmptyOptional()
    {
        // arrange

        // act
        var v = Optional<Tester>.Empty();

        // assert
        Assert.IsNotNull(v);
        Assert.IsFalse(v.HasValue);
    }

    [TestMethod]
    public void OrElse_WithValue_ReturnsValue()
    {
        // arrange
        var v = Optional<string>.Of("test");

        // act
        var result = v.OrElse("none");

        // assert
        Assert.AreEqual("test", result);
    }

    [TestMethod]
    public void OrElse_WithNullValue_ReturnsElseValue()
    {
        // arrange
        var v = Optional<string>.Empty();

        // act
        var result = v.OrElse("none");

        // assert
        Assert.AreEqual("none", result);
    }

    [TestMethod]
    public void OrElseFunc_WithNullValue_ReturnsElseFuncResult()
    {
        // arrange
        var v = Optional<string>.Empty();

        // act
        var result = v.OrElse(() => "func-none");

        // assert
        Assert.AreEqual("func-none", result);
    }

    [TestMethod]
    public void IfPresentOrElse_WithValue_ReturnsValue()
    {
        // arrange
        var v = Optional<string>.Of("test");
        string result = string.Empty;

        // act
        v.IfPresentOrElse(
            value => { result = value; },
            [ExcludeFromCodeCoverage] () => { result = "else"; });

        // assert
        Assert.AreEqual("test", result);
    }

    [TestMethod]
    public void IfPresentOrElse_WithNullValue_ReturnsElseValue()
    {
        // arrange
        var v = Optional<string>.Empty();
        string result = string.Empty;

        // act
        v.IfPresentOrElse(
            [ExcludeFromCodeCoverage] (value) => { result = value; },
            () => { result = "else"; });

        // assert
        Assert.AreEqual("else", result);
    }

    [TestMethod]
    public void IfPresentOrElseTResult_WithValue_ReturnsValue()
    {
        // arrange
        var v = Optional<string>.Of("test");

        // act
        var result = v.IfPresentOrElse<bool>(value => true, [ExcludeFromCodeCoverage] () => false);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IfPresentOrElseTResult_WithNullValue_ReturnsElseValue()
    {
        // arrange
        var v = Optional<string>.Empty();

        // act
        var result = v.IfPresentOrElse<bool>([ExcludeFromCodeCoverage] (value) => true, () => false);

        // assert
        Assert.IsFalse(result);
    }
}
