using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal class EnvironmentBuilder(ILogger<EnvironmentBuilder> logger)
{
    public void SetEnvironmentVariables(string env)
    {
        var envVars = !string.IsNullOrWhiteSpace(env) ? env.Split(',').Select(x => x.Trim()) : Array.Empty<string>();
        foreach (var envVar in envVars.Select(x => x.Split('=').Select(x => x.Trim()).ToList()))
        {
            if (envVar.Count is 1)
            {
                Environment.SetEnvironmentVariable(envVar[0], null, EnvironmentVariableTarget.Process);
                logger.LogDebug($"Set environment flag: {envVar[0]}");
            }
            if (envVar.Count is 2)
            {
                Environment.SetEnvironmentVariable(envVar[0], envVar.ElementAtOrDefault(1), EnvironmentVariableTarget.Process);
                logger.LogDebug($"Set environment variable: {envVar[0]} = {envVar[1]}");
            }
            else
            {
                logger.LogCritical("Environment variables should be in the format: env1=value1,env2=value2,env3");
            }
        }
    }
}
