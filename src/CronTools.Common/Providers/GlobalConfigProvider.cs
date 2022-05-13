using System.Collections.Generic;
using CronTools.Common.Config;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Providers;

public interface IGlobalConfigProvider
{
  Dictionary<string, string> GetGlobalConfig();
}

public class GlobalConfigProvider : IGlobalConfigProvider
{
  private readonly ILoggerAdapter<GlobalConfigProvider> _logger;
  private readonly CronToolConfig _config;
  private readonly IFileAbstraction _file;
  private readonly IPathAbstraction _path;
  private readonly IJsonHelper _jsonHelper;

  public GlobalConfigProvider(
    IConfigProvider configProvider,
    IFileAbstraction file,
    IPathAbstraction path,
    ILoggerAdapter<GlobalConfigProvider> logger,
    IJsonHelper jsonHelper)
  {
    _file = file;
    _path = path;
    _logger = logger;
    _jsonHelper = jsonHelper;
    _config = configProvider.GetConfig();
  }

  public Dictionary<string, string> GetGlobalConfig()
  {
    // TODO: [GlobalConfigProvider.GetGlobalConfig] (TESTS) Add tests
    var filePath = _path.Join(_config.JobsDirectory, "globals.json");
    if (!_file.Exists(filePath))
      return new Dictionary<string, string>();

    _logger.LogInformation("Attempting to load globals file: {path}", filePath);
    var rawJson = _file.ReadAllText(filePath);

    if (!_jsonHelper.TryDeserializeObject(rawJson, out Dictionary<string, string> config))
      return new Dictionary<string, string>();
    
    _logger.LogInformation("Loaded {count} global variables", config.Count);
    return config;
  }
}