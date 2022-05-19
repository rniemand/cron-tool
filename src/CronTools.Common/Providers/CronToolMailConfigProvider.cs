using System.Net.Mail;
using CronTools.Common.Exceptions;
using CronTools.Common.Extensions;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.MailUtils.Config;
using Rn.NetCore.MailUtils.Providers;

namespace CronTools.Common.Providers;

public class CronToolMailConfigProvider : IRnMailConfigProvider
{
  private readonly ILoggerAdapter<CronToolMailConfigProvider> _logger;
  private readonly IGlobalConfigProvider _globalConfigProvider;
  private readonly IConfigProvider _configProvider;
  private readonly IPathAbstraction _path;
  private readonly IDirectoryAbstraction _directory;
  private RnMailConfig? _config;

  public CronToolMailConfigProvider(
    ILoggerAdapter<CronToolMailConfigProvider> logger,
    IGlobalConfigProvider globalConfigProvider,
    IConfigProvider configProvider,
    IPathAbstraction path,
    IDirectoryAbstraction directory)
  {
    _logger = logger;
    _globalConfigProvider = globalConfigProvider;
    _configProvider = configProvider;
    _path = path;
    _directory = directory;
    _config = null;
  }

  public RnMailConfig GetRnMailConfig()
  {
    // TODO: [CronToolMailConfigProvider.GetRnMailConfig] (TESTS) Add tests
    if(_config is null)
      BindConfig();

    if (_config is null)
      throw new GeneralCronToolException("Unable to bind configuration");

    return _config;
  }

  private void BindConfig()
  {
    // TODO: [CronToolMailConfigProvider.BindConfig] (TESTS) Add tests
    _logger.LogDebug("Attempting to bind RnMailConfig");
    var config = _globalConfigProvider.GetGlobalConfig();

    // ReSharper disable once UseObjectOrCollectionInitializer
    var mailConfig = new RnMailConfig();
    mailConfig.Host = config.GetStringValue("mail.host", "smtp.gmail.com");
    mailConfig.Port = config.GetIntValue("mail.port", 587);
    mailConfig.Username = config.GetStringValue("mail.username", string.Empty);
    mailConfig.Password = config.GetStringValue("mail.password", string.Empty);
    mailConfig.FromAddress = config.GetStringValue("mail.fromAddress", string.Empty);
    mailConfig.FromName = config.GetStringValue("mail.fromName", string.Empty);
    mailConfig.Timeout = config.GetIntValue("mail.timeout", 30000);
    mailConfig.TemplateDir = config.GetStringValue("mail.templateDir", "{root}mail-tpl");
    mailConfig.EnableSsl = config.GetBoolValue("mail.enableSsl", true);
    mailConfig.DeliveryFormat = config.GetEnumValue("mail.deliveryFormat", SmtpDeliveryFormat.SevenBit);
    mailConfig.DeliveryMethod = config.GetEnumValue("mail.deliveryMethod", SmtpDeliveryMethod.Network);

    // Handle path formatting
    var cronToolConfig = _configProvider.GetConfig();
    if (mailConfig.TemplateDir.Contains("{root}"))
    {
      mailConfig.TemplateDir = _path.Join(cronToolConfig.RootDirectory,
        mailConfig.TemplateDir.Replace("{root}", ""));

      _logger.LogInformation("Setting mail template dir to: {path}", mailConfig.TemplateDir);
    }

    // Ensure that the mail template folder exists
    if (!_directory.Exists(mailConfig.TemplateDir))
    {
      _logger.LogInformation("Creating mail template dir: {path}", mailConfig.TemplateDir);
      _directory.CreateDirectory(mailConfig.TemplateDir);
    }

    _logger.LogInformation("Created new instance of RnMailConfig!");
    _config = mailConfig;
  }
}
