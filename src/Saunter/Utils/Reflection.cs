using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Saunter.Utils
{
    public static class Reflection
    {
        public static bool HasCustomAttribute<T>(this TypeInfo typeInfo) where T : Attribute
        {
            return typeInfo.GetCustomAttribute<T>() != null;
        }

        public static bool HasCustomAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            return methodInfo.GetCustomAttribute<T>() != null;
        }

        public static bool IsInteger(this Type type)
        {
            if (type == typeof(int)
                || type == typeof(uint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(short)
                || type == typeof(ushort))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsInteger();
            }

            return false;
        }

        public static bool IsNumber(this Type type)
        {
            if (type == typeof(float)
                || type == typeof(double)
                || type == typeof(decimal))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsNumber();
            }

            return false;
        }

        public static bool IsBoolean(this Type type)
        {
            if (type == typeof(bool)) return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsBoolean();
            }

            return false;
        }

        public static bool IsEnumerable(this Type type, out Type elementType)
        {
            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

            if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                var typeArguments = type.GetGenericArguments();
                if (typeArguments.Length != 1)
                {
                    elementType = null;
                    return false;
                }

                elementType = typeArguments.Single();
                return true;
            }

            elementType = null;
            return false;
        }

        public static bool IsEnum(this Type type, out IList<string> members)
        {
            if (type.IsEnum)
            {
                members = type.GetEnumMembers();
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsEnum(out members);
            }

            members = null;
            return false;
        }

        public static IList<string> GetEnumMembers(this Type type)
        {
            var values = new List<string>();
            
            foreach (Enum val in type.GetEnumValues())
            {
                var enumMemberAttribute = val.GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttribute?.Value != null)
                {
                    values.Add(enumMemberAttribute.Value);
                }
                else
                {
                    values.Add(val.ToString());
                }
            }

            return values;
        }
        
        public static T GetCustomAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString()).Single();
            var attribute = memberInfo.GetCustomAttributes<T>().FirstOrDefault();
            return attribute;
        }

        public static bool IsDateTime(this Type type)
        {
            if (type == typeof(DateTime))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsDateTime();
            }

            return false;
        }

        
        
    }
}