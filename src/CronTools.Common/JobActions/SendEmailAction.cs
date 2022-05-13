using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

public class SendEmailAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }
  public string[] RequiredGlobals { get; }

  private readonly ILoggerAdapter<SendEmailAction> _logger;

  public SendEmailAction(ILoggerAdapter<SendEmailAction> logger)
  {
    // TODO: [SendEmailAction] (TESTS) Add tests
    _logger = logger;

    Action = JobStepAction.SendEmail;
    Name = JobStepAction.SendEmail.ToString("G");
    RequiredGlobals = new[]
    {
      "mail.username",
      "mail.password",
      "mail.fromAddress",
      "mail.fromName"
    };

    Args = new Dictionary<string, JobActionArg>
    {
      { "ToAddress", JobActionArg.Email("ToAddress", true) },
      { "ToName", JobActionArg.String("ToName", false) },
      { "Subject", JobActionArg.String("Subject", true) },
      { "Body", JobActionArg.String("Body", true) }
  };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    // TODO: [SendEmailAction.ExecuteAsync] (TESTS) Add tests
    var outcome = new JobStepOutcome();


    /*
     * {
  "mail.host": "smtp.gmail.com",
  "mail.port": 587,
  "mail.username": "
  "mail.password":
  "mail.fromAddress": 
  "mail.fromName": 
}
     */



    Console.WriteLine();
    return outcome.WithSuccess();
  }
}
