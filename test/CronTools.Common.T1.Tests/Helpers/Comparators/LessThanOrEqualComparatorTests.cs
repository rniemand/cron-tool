using System;
using CronTools.Common.Enums;
using CronTools.Common.Helpers.Comparators;
using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.T1.Tests.Helpers.Comparators;

[TestFixture]
public class LessThanOrEqualComparatorTests
{
  [Test]
  public void Compare_GivenCalled_ShouldLog()
  {
    // arrange
    var logger = Substitute.For<ILoggerAdapter<LessThanOrEqualComparator>>();
    var comparator = GetComparator(logger);

    // act
    comparator.Compare((long)1025, "1024");

    // assert
    logger.Received(1).LogDebug("Comparing: {source} <= {target}", (long)1025, "1024");
  }

  [Test]
  public void Compare_GivenUnsupportedType_ShouldLog()
  {
    // arrange
    var logger = Substitute.For<ILoggerAdapter<LessThanOrEqualComparator>>();
    var comparator = GetComparator(logger);

    // act
    comparator.Compare(Array.Empty<string>(), "1024");

    // assert
    logger.Received(1).LogError("Add support for {type}", "String[]");
  }

  [Test]
  public void Compare_GivenUnsupportedType_ShouldReturn_False()
  {
    // arrange
    var comparator = GetComparator();

    // act
    var result = comparator.Compare(Array.Empty<string>(), "1024");

    // assert
    Assert.That(result, Is.False);
  }

  [Test]
  public void Compare_GivenConstructed_ShouldReturn_Comparator() =>
    Assert.That(GetComparator().Comparator, Is.EqualTo(Comparator.LessThanOrEqual));


  [Test(Description = "Sample Expression: 2048 <= 1024")]
  public void Compare_Long_GivenBiggerSourceValue_ShouldReturn_False()
  {
    // arrange
    var comparator = GetComparator();

    // act
    var result = comparator.Compare((long)2048, "1024");

    // assert
    Assert.That(result, Is.False);
  }

  [Test(Description = "Sample Expression: 512 <= 1024")]
  public void Compare_Long_GivenSmallerSourceValue_ShouldReturn_True()
  {
    // arrange
    var comparator = GetComparator();

    // act
    var result = comparator.Compare((long)512, "1024");

    // assert
    Assert.That(result, Is.True);
  }

  [Test(Description = "Sample Expression: 1024 <= 1024")]
  public void Compare_Long_GivenSameSourceValue_ShouldReturn_True()
  {
    // arrange
    var comparator = GetComparator();

    // act
    var result = comparator.Compare((long)1024, "1024");

    // assert
    Assert.That(result, Is.True);
  }


  private static LessThanOrEqualComparator GetComparator(ILoggerAdapter<LessThanOrEqualComparator> logger = null) =>
    new(logger ?? Substitute.For<ILoggerAdapter<LessThanOrEqualComparator>>());
}
