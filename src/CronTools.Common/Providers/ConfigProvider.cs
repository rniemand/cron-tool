using CronTools.Common.Config;
using Microsoft.Extensions.Configuration;
using Rn.NetCore.Common.Abstractions;

namespace CronTools.Common.Providers;

public interface IConfigProvider
{
  CronToolConfig GetConfig();
}

public class ConfigProvider : IConfigProvider
{
  private readonly IConfiguration _configuration;
  private readonly IEnvironmentAbstraction _environment;
  private CronToolConfig? _config;

  public ConfigProvider(
    IConfiguration configuration,
    IEnvironmentAbstraction environment)
  {
    _configuration = configuration;
    _environment = environment;
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

    _config = _config.NormalizePaths(_environment.CurrentDirectory);
    return _config;
  }
}
