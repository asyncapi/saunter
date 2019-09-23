using System;
using System.Reflection;

namespace Saunter.Utils
{
    public static class Reflection
    {
        public static bool HasCustomAttribute<T>(this TypeInfo typeInfo) where T : Attribute
        {
            return typeInfo.GetCustomAttribute<T>() != null;
        }
    }
}