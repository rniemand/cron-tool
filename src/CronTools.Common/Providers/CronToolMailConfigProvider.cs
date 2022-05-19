using CronTools.Common.Extensions;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.MailUtils.Config;
using Rn.NetCore.MailUtils.Providers;

namespace CronTools.Common.Providers;

public class CronToolMailConfigProvider : IRnMailConfigProvider
{
  private readonly ILoggerAdapter<CronToolMailConfigProvider> _logger;
  private readonly IGlobalConfigProvider _globalConfigProvider;


  public CronToolMailConfigProvider(
    ILoggerAdapter<CronToolMailConfigProvider> logger,
    IGlobalConfigProvider globalConfigProvider)
  {
    _logger = logger;
    _globalConfigProvider = globalConfigProvider;
  }

  public RnMailConfig GetRnMailConfig()
  {
    var config = _globalConfigProvider.GetGlobalConfig();

    var rnMailConfig = new RnMailConfig();

    // mail.deliveryFormat | deliveryFormat | SevenBit
    // mail.deliveryMethod | deliveryMethod | Network
    // mail.enableSsl | enableSsl | true
    // mail.encoding | encoding | UTF8


    rnMailConfig.Host = config.GetStringValue("mail.host", "smtp.gmail.com");
    rnMailConfig.Port = config.GetIntValue("mail.port", 587);
    rnMailConfig.Username = config.GetStringValue("mail.username", string.Empty);
    rnMailConfig.Password = config.GetStringValue("mail.password", string.Empty);
    rnMailConfig.FromAddress = config.GetStringValue("mail.fromAddress", string.Empty);
    rnMailConfig.FromName = config.GetStringValue("mail.fromName", string.Empty);
    rnMailConfig.Timeout = config.GetIntValue("mail.timeout", 30000);
    rnMailConfig.TemplateDir = config.GetStringValue("mail.templateDir", "{root}mail-tpl");


    return null;
  }
}
