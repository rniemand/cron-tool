using System.Net.Mail;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Factories;

public interface ISmtpClientFactory
{
  SmtpClient CreateNew(string host, int port);
}

public class SmtpClientFactory : ISmtpClientFactory
{
  private readonly ILoggerAdapter<SmtpClientFactory> _logger;

  public SmtpClientFactory(ILoggerAdapter<SmtpClientFactory> logger)
  {
    // TODO: [SmtpClientFactory] (TESTS) Add tests
    _logger = logger;
  }

  public SmtpClient CreateNew(string host, int port)
  {
    // TODO: [SmtpClientFactory.CreateNew] (TESTS) Add tests
    return new SmtpClient(host, port)
    {
      DeliveryFormat = SmtpDeliveryFormat.SevenBit,
      DeliveryMethod = SmtpDeliveryMethod.Network,
      EnableSsl = true,
      PickupDirectoryLocation = null,
      TargetName = null,
      Timeout = 30000,
      UseDefaultCredentials = false
    };
  }
}
