using System;
using CronTools.Common.Models;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Helpers;

public interface IJobStepHelper
{
  void InitializeSteps(JobConfig jobConfig);
}

public class JobStepHelper : IJobStepHelper
{
  public void InitializeSteps(JobConfig jobConfig)
  {
    var currentStepNumber = 1;

    foreach (var jobStep in jobConfig.Steps)
    {
      jobStep.JobName = jobConfig.Name;
      SetStepId(jobStep, currentStepNumber++);
      ProcessStepCondition(jobStep);
    }
  }

  private static void SetStepId(JobStepConfig jobStep, int stepNumber)
  {
    jobStep.StepNumber = stepNumber;

    if (!string.IsNullOrWhiteSpace(jobStep.StepId))
      return;

    jobStep.StepId = "step_" + jobStep.StepNumber.ToString("D").PadLeft(2, '0');
  }

  private static void ProcessStepCondition(JobStepConfig jobStep)
  {
    var condition = jobStep.Condition;

    if(condition is null)
      return;

    foreach (var rawExpression in condition.RawExpressions)
    {
      if(string.IsNullOrWhiteSpace(rawExpression))
        continue;

      condition.Expressions.Add(ParseCondition(rawExpression));
    }
  }

  private static ConditionExpression ParseCondition(string rawCondition)
  {
    var parsed = new ConditionExpression
    {
      RawExpression = rawCondition,
      IsValid = false
    };

    // (.*?) ([^a-zA-Z0-9\s\.]+) (.*)
    const string regex = "(.*?) ([^a-zA-Z0-9\\s\\.]+) (.*)";
    if (!rawCondition.MatchesRegex(regex))
      return parsed;

    var regexMatch = rawCondition.GetRegexMatch(regex);

    parsed.Property = regexMatch.Groups[1].Value;
    parsed.Comparator = CastHelper.ToComparator(regexMatch.Groups[2].Value);
    parsed.Value = regexMatch.Groups[3].Value;
    parsed.IsValid = true;

    return parsed;
  }
}
