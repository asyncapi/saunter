using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

        public static bool IsGuid(this Type type)
        {
            if (type == typeof(Guid)) return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsGuid();
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

        public static bool IsTimeSpan(this Type type)
        {
            if (type == typeof(TimeSpan))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var typeArgument = type.GetGenericArguments().Single();
                return typeArgument.IsTimeSpan();
            }

            return false;
        }

        public static string GetTitle(this MemberInfo memberInfo)
        {
            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name;
        }

        public static string GetDescription(this MemberInfo memberInfo)
        {
            var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute?.Description != null)
            {
                return displayAttribute.Description;
            }

            var xmlSummary = memberInfo.GetXmlDocsSummary();
            if (!string.IsNullOrEmpty(xmlSummary))
            {
                return xmlSummary;
            }

            return null;
        }

        public static decimal? GetMinimum(this MemberInfo memberInfo)
        {
            var rangeAttribute = memberInfo.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute?.Minimum != null && decimal.TryParse(rangeAttribute.Minimum.ToString(), out var minimum))
            {
                return minimum;
            }

            return null;
        }

        public static decimal? GetMaximum(this MemberInfo memberInfo)
        {
            var rangeAttribute = memberInfo.GetCustomAttribute<RangeAttribute>();
            if (rangeAttribute?.Maximum != null && decimal.TryParse(rangeAttribute.Maximum.ToString(), out var maximum))
            {
                return maximum;
            }

            return null;
        }

        public static int? GetMinItems(this MemberInfo memberInfo)
        {
            var minLengthAttribute = memberInfo.GetCustomAttribute<MinLengthAttribute>();
            return minLengthAttribute?.Length;
        }

        public static int? GetMaxItems(this MemberInfo memberInfo)
        {
            var maxLengthAttribute = memberInfo.GetCustomAttribute<MaxLengthAttribute>();
            return maxLengthAttribute?.Length;
        }

        public static int? GetMinLength(this MemberInfo memberInfo)
        {
            var minLengthAttribute = memberInfo.GetCustomAttribute<MinLengthAttribute>();
            if (minLengthAttribute != null)
            {
                return minLengthAttribute.Length;
            }

            var stringLengthAttribute = memberInfo.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MinimumLength;
        }

        public static int? GetMaxLength(this MemberInfo memberInfo)
        {
            var maxLengthAttribute = memberInfo.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null)
            {
                return maxLengthAttribute.Length;
            }

            var stringLengthAttribute = memberInfo.GetCustomAttribute<StringLengthAttribute>();
            return stringLengthAttribute?.MaximumLength;
        }

        public static bool? GetIsUniqueItems(this PropertyInfo prop)
        {
            return GetIsUniqueItems(prop as MemberInfo);
        }

        public static bool? GetIsUniqueItems(this MemberInfo memberInfo)
        {
            var type = GetUnderlyingType(memberInfo);

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

        public static string GetPattern(this MemberInfo memberInfo)
        {
            var regexAttribute = memberInfo.GetCustomAttribute<RegularExpressionAttribute>();
            return regexAttribute?.Pattern;
        }

        public static bool GetIsRequired(this MemberInfo memberInfo)
        {
            var requiredAttribute = memberInfo.GetCustomAttribute<RequiredAttribute>();
            return requiredAttribute != null;
        }

        public static string GetExample(this MemberInfo memberInfo)
        {
            var xmlExample = memberInfo.GetXmlDocsTag("example");
            if (!String.IsNullOrEmpty(xmlExample))
            {
                return xmlExample;
            }

            return null;
        }

        public static Type GetUnderlyingType(MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type FieldInfo or PropertyInfo"
                    );
            }
        }
    }
}