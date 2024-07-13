// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AsyncAPI.Saunter.Generator.Cli.Tests;

public static class ExpectedSpecFiles
{
    public static string Json_v2_6 => File.ReadAllText("Specs/streetlights_v2.6.json");

    public static string Yml_v2_6 => File.ReadAllText("Specs/streetlights_v2.6.yml");
}
