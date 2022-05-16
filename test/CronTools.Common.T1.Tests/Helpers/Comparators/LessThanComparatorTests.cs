using CronTools.Common.Helpers.Comparators;
using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.T1.Tests.Helpers.Comparators;

[TestFixture]
public class LessThanComparatorTests
{
  [Test(Description = "Sample Expression: 1025 < 1024")]
  public void Compare_Long_GivenBiggerSourceValue_ShouldReturn_False()
  {
    // arrange
    var lessThan = GetComparator();

    // act
    var result = lessThan.Compare((long) 1025, "1024");

    // assert
    Assert.That(result, Is.False);
  }

  [Test(Description = "Sample Expression: 1024 < 1024")]
  public void Compare_Long_GivenSameSourceValue_ShouldReturn_False()
  {
    // arrange
    var lessThan = GetComparator();

    // act
    var result = lessThan.Compare((long)1024, "1024");

    // assert
    Assert.That(result, Is.False);
  }

  [Test(Description = "Sample Expression: 512 < 1024")]
  public void Compare_Long_GivenSmallerSourceValue_ShouldReturn_True()
  {
    // arrange
    var lessThan = GetComparator();

    // act
    var result = lessThan.Compare((long)512, "1024");

    // assert
    Assert.That(result, Is.True);
  }

  private static LessThanComparator GetComparator(ILoggerAdapter<LessThanComparator> logger = null)
  {
    return new LessThanComparator(logger ?? Substitute.For<ILoggerAdapter<LessThanComparator>>());
  }
}
