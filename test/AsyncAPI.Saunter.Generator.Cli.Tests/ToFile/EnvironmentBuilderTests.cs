using System.Collections;
using AsyncAPI.Saunter.Generator.Cli.ToFile;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Community.Logging;
using Shouldly;
using Xunit.Abstractions;

namespace AsyncAPI.Saunter.Generator.Cli.Tests.ToFile;

public class EnvironmentBuilderTests : IDisposable
{
    private readonly IDictionary _variablesBefore = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
    private readonly EnvironmentBuilder _environment;
    private readonly ILogger<EnvironmentBuilder> _logger;

    public EnvironmentBuilderTests()
    {
        this._logger = Substitute.For<ILogger<EnvironmentBuilder>>();
        this._environment = new EnvironmentBuilder(this._logger);
    }

    private Dictionary<string, string> GetAddedEnvironmentVariables()
    {
        var after = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
        return after.Cast<DictionaryEntry>().ExceptBy(this._variablesBefore.Keys.Cast<string>(), x => x.Key).ToDictionary(x => x.Key.ToString(), x => x.Value?.ToString());
    }

    public void Dispose()
    {
        foreach (var variable in this.GetAddedEnvironmentVariables())
        {
            Environment.SetEnvironmentVariable(variable.Key, null, EnvironmentVariableTarget.Process);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void EmptyEnvStringProvided(string env)
    {
        this._environment.SetEnvironmentVariables(env);

        this._logger.ReceivedCalls().Count().ShouldBe(0);
        this.GetAddedEnvironmentVariables().ShouldBeEmpty();
    }

    [Theory]
    [InlineData("env1=val1", 1)]
    [InlineData("a=b,b=c", 2)]
    public void ValidEnvStringProvided(string env, int expectedSets)
    {
        this._environment.SetEnvironmentVariables(env);

        this._logger.Received(expectedSets).CallToLog(LogLevel.Debug);
        this.GetAddedEnvironmentVariables().ShouldNotBeEmpty();
    }

    [Theory]
    [InlineData(",", 2)]
    [InlineData(",,,,", 5)]
    [InlineData("=a", 1)]
    [InlineData("b", 1)]
    [InlineData("=", 1)]
    [InlineData("====", 1)]
    public void InvalidEnvStringProvided(string env, int expectedSets)
    {
        this._environment.SetEnvironmentVariables(env);

        this._logger.Received(expectedSets).CallToLog(LogLevel.Critical);
        this.GetAddedEnvironmentVariables().ShouldBeEmpty();
    }

    [Fact]
    public void ValidateEnvValues()
    {
        this._environment.SetEnvironmentVariables("ENV=1,,Test=two");

        Environment.GetEnvironmentVariable("ENV").ShouldBe("1");
        Environment.GetEnvironmentVariable("Test").ShouldBe("two");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public void EmptyValueDeletesEnvValue(string value)
    {
        this._environment.SetEnvironmentVariables($"ENV=1,,ENV={value}");

        Environment.GetEnvironmentVariable("ENV").ShouldBe(null);
    }
}
