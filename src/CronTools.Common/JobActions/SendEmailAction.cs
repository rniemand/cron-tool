using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Helpers;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.MailUtils.Builders;
using Rn.NetCore.MailUtils.Factories;
using Rn.NetCore.MailUtils.Helpers;

namespace CronTools.Common.JobActions;

public class SendEmailAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }
  public string[] RequiredGlobals { get; }

  private readonly ILoggerAdapter<SendEmailAction> _logger;
  private readonly ISmtpClientFactory _smtpClientFactory;
  private readonly IMailMessageBuilderFactory _messageBuilderFactory;
  private readonly IMailTemplateHelper _mailTemplateHelper;

  public SendEmailAction(
    ILoggerAdapter<SendEmailAction> logger,
    ISmtpClientFactory smtpClientFactory,
    IMailMessageBuilderFactory messageBuilderFactory,
    IMailTemplateHelper mailTemplateHelper)
  {
    // TODO: [SendEmailAction] (TESTS) Add tests
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
      {"Template", JobActionArg.String("Template", false, "default")}
    };

    _logger = logger;
    _smtpClientFactory = smtpClientFactory;
    _messageBuilderFactory = messageBuilderFactory;
    _mailTemplateHelper = mailTemplateHelper;
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    // TODO: [SendEmailAction.ExecuteAsync] (TESTS) Add tests
    var outcome = new JobStepOutcome();

    // Ensure that we are able to resolve the requested mail template
    var templateName = argResolver.ResolveString(jobContext, stepContext, Args["Template"]);
    var templateBuilder = _mailTemplateHelper.GetTemplateBuilder(templateName);
    if (!templateBuilder.TemplateFound)
    {
      _logger.LogError("Unable to resolve mail template {name}", templateName);
      return outcome.WithError($"Unable to resolve mail template {templateName}");
    }

    try
    {
      var message = CreateMailMessageBuilder(jobContext, stepContext, argResolver)
        .WithSubject(argResolver.ResolveString(jobContext, stepContext, Args["Subject"]))
        .WithHtmlBody(AppendPlaceholders(templateBuilder, jobContext))
        .Build();

      var messageBody = message.Body;

      var smtpClient = _smtpClientFactory.Create();


      Console.WriteLine();
      Console.WriteLine();
      var mailMessage = CreateMessage(jobContext, stepContext, argResolver);
      await CreateMailClient(jobContext).SendMailAsync(mailMessage);
      return outcome.WithSuccess();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error sending email: {message}", ex.Message);
      return outcome.WithFailed();
    }
  }

  private MailMessageBuilder CreateMailMessageBuilder(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    // TODO: [SendEmailAction.CreateMailMessageBuilder] (TESTS) Add tests
    var builder = _messageBuilderFactory.Create();

    var toAddress = argResolver.ResolveString(jobContext, stepContext, Args["ToAddress"]);
    var toName = argResolver.ResolveString(jobContext, stepContext, Args["ToName"]);

    if (string.IsNullOrWhiteSpace(toName))
      toName = toAddress;

    builder.WithTo(toAddress, toName);
    return builder;
  }

  private static MailTemplateBuilder AppendPlaceholders(MailTemplateBuilder builder, RunningJobContext jobContext)
  {
    // TODO: [SendEmailAction.AppendPlaceholders] (TESTS) Add tests
    foreach (var state in jobContext.State)
    {
      builder.AddPlaceHolder($"state.{state.Key}", state.Value);
    }

    foreach (var variable in jobContext.Variables)
    {
      builder.AddPlaceHolder($"var.{variable.Key}", variable.Value);
    }

    foreach (var global in jobContext.Globals)
    {
      builder.AddPlaceHolder($"global.{global.Key}", global.Value);
    }

    return builder;
  }





  private static SmtpClient CreateMailClient(RunningJobContext jobContext)
  {
    var mailHost = jobContext.GetGlobal("mail.host", "smtp.gmail.com");
    var mailPort = CastHelper.AsInt(jobContext.GetGlobal("mail.port", "587"), 587);
    var mailUsername = jobContext.GetGlobal("mail.username", string.Empty);
    var mailPassword = jobContext.GetGlobal("mail.password", string.Empty);

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

    return smtpClient;
  }

  private MailMessage CreateMessage(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    var fromAddress = jobContext.GetGlobal("mail.fromAddress", string.Empty);
    var fromName = jobContext.GetGlobal("mail.fromName", string.Empty);

    var toAddress = argResolver.ResolveString(jobContext, stepContext, Args["ToAddress"]);
    var toName = argResolver.ResolveString(jobContext, stepContext, Args["ToName"]);
    var subject = argResolver.ResolveString(jobContext, stepContext, Args["Subject"]);
    var body = argResolver.ResolveString(jobContext, stepContext, Args["Body"]);

    var mailMessage = new MailMessage();
    mailMessage.From = new MailAddress(fromAddress, fromName, Encoding.UTF8);
    mailMessage.To.Add(new MailAddress(toAddress, toName, Encoding.UTF8));
    mailMessage.Subject = subject;
    mailMessage.SubjectEncoding = Encoding.UTF8;
    mailMessage.Body = body;
    mailMessage.IsBodyHtml = true;
    mailMessage.BodyEncoding = Encoding.UTF8;

    return mailMessage;
  }
}
