using CronTools.Common.Config;
using Microsoft.Extensions.Configuration;

namespace CronTools.Common.Providers
{
  public interface IConfigProvider
  {
    CronToolConfig GetConfig();
  }

  public class ConfigProvider : IConfigProvider
  {
    private readonly IConfiguration _configuration;
    private CronToolConfig _config;

    public ConfigProvider(IConfiguration configuration)
    {
      // TODO: [TESTS] (ConfigProvider.ConfigProvider) Add tests
      _configuration = configuration;
      _config = null;
    }

    public CronToolConfig GetConfig()
    {
      // TODO: [TESTS] (ConfigProvider.GetConfig) Add tests
      if (_config is not null)
        return _config;

      _config = new CronToolConfig();
      var section = _configuration.GetSection(CronToolConfig.Key);
      
      if (section.Exists())
        section.Bind(_config);

      return _config;
    }
  }
}
