using System;
using System.Collections.Generic;
using CronTools.Common.Config;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Providers;

public interface IGlobalConfigProvider
{
  Dictionary<string, object> GetGlobalConfig();
}

public class GlobalConfigProvider : IGlobalConfigProvider
{
  private readonly ILoggerAdapter<GlobalConfigProvider> _logger;
  private readonly CronToolConfig _config;
  private readonly IFileAbstraction _file;
  private readonly IPathAbstraction _path;

  public GlobalConfigProvider(
    IConfigProvider configProvider,
    IFileAbstraction file,
    IPathAbstraction path,
    ILoggerAdapter<GlobalConfigProvider> logger)
  {
    _file = file;
    _path = path;
    _logger = logger;
    _config = configProvider.GetConfig();
  }

  public Dictionary<string, object> GetGlobalConfig()
  {
    // TODO: [GlobalConfigProvider.GetGlobalConfig] (TESTS) Add tests
    var config = new Dictionary<string, object>();

    var filePath = _path.Join(_config.JobsDirectory, "globals.json");
    if (!_file.Exists(filePath))
      return config;

    _logger.LogInformation("Attempting to load globals file: {path}", filePath);




    Console.WriteLine();
    return config;
  }
}
