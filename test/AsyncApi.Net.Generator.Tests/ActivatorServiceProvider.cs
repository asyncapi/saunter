using System;

namespace AsyncApi.Net.Generator.Tests;

public class ActivatorServiceProvider : IServiceProvider
{
    public static readonly IServiceProvider Instance = new ActivatorServiceProvider();

    public object GetService(Type serviceType)
    {
        return Activator.CreateInstance(serviceType);
    }
}