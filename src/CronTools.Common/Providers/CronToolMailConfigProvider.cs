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
    var globalConfig = _globalConfigProvider.GetGlobalConfig();

    // mail.host | host | smtp.gmail.com
    // mail.port | port | 587
    // mail.username | username
    // mail.password | password
    // mail.fromAddress | fromAddress
    // mail.fromName | fromName
    // mail.deliveryFormat | deliveryFormat | SevenBit
    // mail.deliveryMethod | deliveryMethod | Network
    // mail.enableSsl | enableSsl | true
    // mail.timeout | timeout | 30000
    // mail.encoding | encoding | UTF8
    // mail.templateDir | templateDir | {root}mail-tpl


    return null;
  }
}
