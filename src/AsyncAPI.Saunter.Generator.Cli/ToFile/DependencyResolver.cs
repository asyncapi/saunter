using System.Reflection;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal static class DependencyResolver
{
    public static void Init(string startupAssemblyBasePath)
    {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var requestedAssembly = new AssemblyName(args.Name);
            var fullPath = Path.Combine(startupAssemblyBasePath, $"{requestedAssembly.Name}.dll");
            if (File.Exists(fullPath))
            {
                var assembly = Assembly.LoadFile(fullPath);
                return assembly;
            }
            Console.WriteLine($"Could not resolve assembly: {args.Name}, requested by {args.RequestingAssembly?.FullName}");
            return default;
        };
    }
}
