namespace AsyncAPI.Saunter.Generator.Cli.Tests;

public static class ExpectedSpecFiles
{
    public static string Json_v2_6 => File.ReadAllText("Specs/streetlights_v2.6.json");

    public static string Yml_v2_6 => File.ReadAllText("Specs/streetlights_v2.6.yml");
}
