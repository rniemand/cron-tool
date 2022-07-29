using System.Collections.Generic;
using CronTools.Common.Enums;
using CronTools.Common.Helpers;
using CronTools.Common.Helpers.Comparators;
using CronTools.Common.T1.Tests.TestSupport.Builders;
using NSubstitute;
using NUnit.Framework;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.T1.Tests.Helpers;

[TestFixture]
public class ConditionHelperTests
{
  [Test]
  public void CanRunJobStep_GivenLessThanOrEqualExpression_ShouldCall_LessThanOrEqualComparator()
  {
    // arrange
    var comparator = Substitute.For<IComparator>();
    var argHelper = Substitute.For<IJobActionArgHelper>();

    var runningJobContext = new RunningJobContextBuilder()
      .WithStateValue("backupFile.fileSize", 2048)
      .WithVariable("ThresholdBackupSize", 1024)
      .Build();

    var runningStepContext = new RunningStepContextBuilder()
      .WithCondition(builder => builder
        .WithExpression(expressionBuilder => expressionBuilder
          .WithComparator(Comparator.LessThanOrEqual)
          .WithProperty("backupFile.fileSize")
          .WithValue("${var:ThresholdBackupSize}")))
      .Build();

    comparator.Comparator.Returns(Comparator.LessThanOrEqual);
    argHelper
      .ProcessExpressionValue(runningJobContext, "${var:ThresholdBackupSize}")
      .Returns("1024");

    var conditionHelper = GetConditionHelper(
      comparators: new List<IComparator> {comparator},
      jobActionArgHelper: argHelper);

    // act
    conditionHelper.CanRunJobStep(runningJobContext, runningStepContext);

    // assert
    comparator.Received(1).Compare(2048, "1024");
  }


  // Internal methods
  private static ConditionHelper GetConditionHelper(
    ILoggerAdapter<ConditionHelper> logger = null,
    IJobActionArgHelper jobActionArgHelper = null,
    List<IComparator> comparators = null)
  {
    return new ConditionHelper(
      logger ?? Substitute.For<ILoggerAdapter<ConditionHelper>>(),
      jobActionArgHelper ?? Substitute.For<IJobActionArgHelper>(),
      comparators ?? new List<IComparator>());
  }
}
