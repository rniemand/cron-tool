using CronTools.Common.Helpers;
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


    rnMailConfig.Host = ConfigHelper.GetString(config, "mail.host", "smtp.gmail.com");
    rnMailConfig.Port = ConfigHelper.GetInt(config, "mail.port", 587);
    rnMailConfig.Username = ConfigHelper.GetString(config, "mail.username", string.Empty);
    rnMailConfig.Password = ConfigHelper.GetString(config, "mail.password", string.Empty);
    rnMailConfig.FromAddress = ConfigHelper.GetString(config, "mail.fromAddress", string.Empty);
    rnMailConfig.FromName = ConfigHelper.GetString(config, "mail.fromName", string.Empty);
    rnMailConfig.Timeout = ConfigHelper.GetInt(config, "mail.timeout", 30000);
    rnMailConfig.TemplateDir = ConfigHelper.GetString(config, "mail.templateDir", "{root}mail-tpl");


    return null;
  }
}
