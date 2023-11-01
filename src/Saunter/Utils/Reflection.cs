using System;
using System.Reflection;

namespace Saunter.Utils;

internal static class Reflection
{
    public static bool HasCustomAttribute<T>(this Type type) where T : Attribute
    {
        return type.GetCustomAttribute<T>() != null;
    }
}