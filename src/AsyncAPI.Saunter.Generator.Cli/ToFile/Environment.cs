using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal interface IEnvironmentBuilder
{
    void SetEnvironmentVariables(string env);
}

internal class EnvironmentBuilder(ILogger<EnvironmentBuilder> logger) : IEnvironmentBuilder
{
    public void SetEnvironmentVariables(string env)
    {
        var envVars = !string.IsNullOrWhiteSpace(env) ? env.Split(',').Select(x => x.Trim()) : Array.Empty<string>();
        var keyValues = envVars.Select(x => x.Split('=').Select(x => x.Trim()).ToList());
        foreach (var envVar in keyValues)
        {
            if (envVar.Count == 2 && !string.IsNullOrWhiteSpace(envVar[0]))
            {
                Environment.SetEnvironmentVariable(envVar[0], envVar[1], EnvironmentVariableTarget.Process);
                logger.LogDebug($"Set environment variable: {envVar[0]} = {envVar[1]}");
            }
            else
            {
                logger.LogCritical("Environment variables should be in the format: env1=value1,env2=value2");
            }
        }
    }
}
