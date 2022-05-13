using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Factories;
using CronTools.Common.Helpers;
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
  private readonly ISmtpClientFactory _smtpClientFactory;

  public SendEmailAction(
    ILoggerAdapter<SendEmailAction> logger,
    ISmtpClientFactory smtpClientFactory)
  {
    // TODO: [SendEmailAction] (TESTS) Add tests
    _logger = logger;
    _smtpClientFactory = smtpClientFactory;

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
      {"ToAddress", JobActionArg.Email("ToAddress", true)},
      {"ToName", JobActionArg.String("ToName", false)},
      {"Subject", JobActionArg.String("Subject", true)},
      {"Body", JobActionArg.String("Body", true)}
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    // TODO: [SendEmailAction.ExecuteAsync] (TESTS) Add tests
    var outcome = new JobStepOutcome();

    var mailHost = jobContext.GetGlobal("mail.host", "smtp.gmail.com");
    var mailPort = CastHelper.AsInt(jobContext.GetGlobal("mail.port", "587"), 587);
    var mailUsername = jobContext.GetGlobal("mail.username", string.Empty);
    var mailPassword = jobContext.GetGlobal("mail.password", string.Empty);
    var fromAddress = jobContext.GetGlobal("mail.fromAddress", string.Empty);
    var fromName = jobContext.GetGlobal("mail.fromName", string.Empty);

    var toAddress = argResolver.ResolveString(jobContext, stepContext, Args["ToAddress"]);
    var toName = argResolver.ResolveString(jobContext, stepContext, Args["ToName"]);
    var subject = argResolver.ResolveString(jobContext, stepContext, Args["Subject"]);
    var body = argResolver.ResolveString(jobContext, stepContext, Args["Body"]);

    var smtpClient = new SmtpClient(mailHost, mailPort)
    {
      DeliveryFormat = SmtpDeliveryFormat.SevenBit,
      DeliveryMethod = SmtpDeliveryMethod.Network,
      EnableSsl = true,
      PickupDirectoryLocation = null,
      TargetName = null,
      Timeout = 30000,
      UseDefaultCredentials = false
    };


    smtpClient.Credentials = new NetworkCredential(mailUsername, mailPassword);

    var mailMessage = new MailMessage();
    mailMessage.From = new MailAddress(fromAddress, fromName, Encoding.UTF8);
    mailMessage.To.Add(new MailAddress(toAddress, toName, Encoding.UTF8));
    mailMessage.Subject = subject;
    mailMessage.SubjectEncoding = Encoding.UTF8;
    mailMessage.Body = body;
    mailMessage.IsBodyHtml = true;
    mailMessage.BodyEncoding = Encoding.UTF8;

    await smtpClient.SendMailAsync(mailMessage);
    

    return outcome.WithSuccess();
  }
}
