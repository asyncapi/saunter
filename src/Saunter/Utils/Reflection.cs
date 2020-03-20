using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Namotion.Reflection;

namespace Saunter.Utils
{
    internal static class Reflection
    {
        public static bool HasCustomAttribute<T>(this TypeInfo typeInfo) where T : Attribute
        {
            return typeInfo.GetCustomAttribute<T>() != null;
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
            // Special case for string which is also an IEnumerable<char>,
            // but we never want to treat it that way when documenting types.
            if (type == typeof(string))
            {
                elementType = null;
                return false;
            }
            
            // Either the type will be IEnumerable<T> or will implement IEnumerable<T>,
            // we need to get the IEnumerable type definition so we can get the type arguments.
            var enumerableType = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                ? type
                : type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            
            if (enumerableType != null)
            {
                var typeArguments = enumerableType.GetGenericArguments();
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

        private static IList<string> GetEnumMembers(this Type type)
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
        
        private static T GetCustomAttribute<T>(this Enum enumValue) where T : Attribute
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

        public static string GetTitle(this PropertyInfo prop)
        {
            var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name;
        }

        public static string GetDescription(this PropertyInfo prop)
        {
            var descriptionAttribute = prop.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute?.Description != null)
            {
                return displayAttribute.Description;
            }

            var xmlSummary = prop.GetXmlDocsSummary();
            if (!string.IsNullOrEmpty(xmlSummary))
            {
                return xmlSummary;
            }

            return null;
        }

        public static decimal? GetMinimum(this PropertyInfo prop)
        {
            var rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute?.Minimum != null && decimal.TryParse(rangeAttribute.Minimum.ToString(), out var minimum))
            {
                return minimum;
            }

            return null;
        }

        public static decimal? GetMaximum(this PropertyInfo prop)
        {
            var rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute?.Maximum != null && decimal.TryParse(rangeAttribute.Maximum.ToString(), out var maximum))
            {
                return maximum;
            }

            return null;
        }

        public static int? GetMinItems(this PropertyInfo prop)
        {
            var minLengthAttribute = prop.GetCustomAttribute<MinLengthAttribute>();
            return minLengthAttribute?.Length;
        }

        public static int? GetMaxItems(this PropertyInfo prop)
        {
            var maxLengthAttribute = prop.GetCustomAttribute<MaxLengthAttribute>();
            return maxLengthAttribute?.Length;
        }

        public static int? GetMinLength(this PropertyInfo prop)
        {
            var minLengthAttribute = prop.GetCustomAttribute<MinLengthAttribute>();
            if (minLengthAttribute != null)
            {
                return minLengthAttribute.Length;
            }

            var stringLengthAttribute = prop.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MinimumLength;
        }
        
        public static int? GetMaxLength(this PropertyInfo prop)
        {
            var maxLengthAttribute = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null)
            {
                return maxLengthAttribute.Length;
            }

            var stringLengthAttribute = prop.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MaximumLength;
        }
        
        public static bool? GetIsUniqueItems(this PropertyInfo prop)
        {
            var type = prop.PropertyType;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ISet<>))
            {
                return true;
            }

            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISet<>)))
            {
                return true;
            }

            return false;
        }

        public static string GetPattern(this PropertyInfo prop)
        {
            var regexAttribute = prop.GetCustomAttribute<RegularExpressionAttribute>();
            return regexAttribute?.Pattern;
        }

        public static bool GetIsRequired(this PropertyInfo prop)
        {
            var requiredAttribute = prop.GetCustomAttribute<RequiredAttribute>();
            return requiredAttribute != null;
        }

        public static string GetExample(this PropertyInfo prop)
        {
            var xmlExample = prop.GetXmlDocsTag("example");
            if (!string.IsNullOrEmpty(xmlExample))
            {
                return xmlExample;
            }

            return null;
        }
    }
}