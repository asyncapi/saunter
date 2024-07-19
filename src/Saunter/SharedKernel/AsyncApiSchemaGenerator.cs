using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LEGO.AsyncAPI.Models;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.SharedKernel
{
    internal class AsyncApiSchemaGenerator : IAsyncApiSchemaGenerator
    {
        public AsyncApiSchema? Generate(Type? type)
        {
            return GenerateBranch(type, new());
        }

        private static AsyncApiSchema? GenerateBranch(Type? type, HashSet<Type> parents)
        {
            if (type is null)
            {
                return null;
            }

            var typeInfo = type.GetTypeInfo();

            var schema = new AsyncApiSchema
            {
                Nullable = !typeInfo.IsValueType,
            };

            if (typeInfo.IsGenericType)
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(typeInfo.GenericTypeArguments);

                if (typeInfo == nullableType)
                {
                    schema.Nullable = true;
                    typeInfo = typeInfo.GenericTypeArguments[0].GetTypeInfo();
                }
            }

            var name = ToNameCase(typeInfo.Name);

            if (!parents.Add(type))
            {
                schema.Reference = new()
                {
                    Id = name,
                    Type = ReferenceType.Schema,
                };

                return schema;
            }

            schema.Title = name;
            schema.Type = MapJsonTypeToSchemaType(typeInfo);

            if (schema.Type is not SchemaType.Object and not SchemaType.Array)
            {
                if (typeInfo.IsEnum)
                {
                    schema.Format = "enum";
                    schema.Enum = typeInfo
                        .GetEnumNames()
                        .Select(e => new AsyncApiAny(e))
                        .ToList();
                }
                else
                {
                    schema.Format = schema.Title;
                }

                return schema;
            }

            schema.Properties = typeInfo
                .DeclaredProperties
                .Where(p => p.GetMethod is not null && !p.GetMethod.IsStatic)
                .ToDictionary(
                    prop => ToNameCase(prop.Name),
                    prop => GenerateBranch(prop.PropertyType.GetTypeInfo(), parents.ToHashSet()));

            return schema;
        }

        private static string ToNameCase(string name)
        {
            return char.ToLowerInvariant(name[0]) + name[1..];
        }

        private static readonly TypeInfo s_boolTypeInfo = typeof(bool).GetTypeInfo();

        private static readonly TypeInfo[] s_stringTypeInfos = new TypeInfo[]
        {
            typeof(string).GetTypeInfo(),
            typeof(DateTime).GetTypeInfo(),
            typeof(DateTimeOffset).GetTypeInfo(),
            typeof(TimeSpan).GetTypeInfo(),
            typeof(Guid).GetTypeInfo(),
            typeof(Uri).GetTypeInfo(),
            typeof(DateOnly).GetTypeInfo(),
            typeof(TimeOnly).GetTypeInfo(),
        };

        private static readonly TypeInfo[] s_intergerTypeInfos = new TypeInfo[]
        {
            typeof(byte).GetTypeInfo(),
            typeof(short).GetTypeInfo(),
            typeof(int).GetTypeInfo(),
            typeof(long).GetTypeInfo(),
            typeof(uint).GetTypeInfo(),
            typeof(ushort).GetTypeInfo(),
            typeof(ulong).GetTypeInfo(),
        };

        private static readonly TypeInfo[] s_floatTypeInfos = new TypeInfo[]
        {
            typeof(float).GetTypeInfo(),
            typeof(decimal).GetTypeInfo(),
            typeof(double).GetTypeInfo(),
        };

        private static SchemaType? MapJsonTypeToSchemaType(TypeInfo typeInfo)
        {
            if (typeInfo == s_boolTypeInfo)
            {
                return SchemaType.Boolean;
            }

            if (typeInfo.IsEnum)
            {
                return SchemaType.String;
            }

            if (s_stringTypeInfos.Contains(typeInfo))
            {
                return SchemaType.String;
            }

            if (s_intergerTypeInfos.Contains(typeInfo))
            {
                return SchemaType.Integer;
            }

            if (s_floatTypeInfos.Contains(typeInfo))
            {
                return SchemaType.Number;
            }

            if (typeInfo.IsArray || (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                return SchemaType.Array;
            }

            return SchemaType.Object;
        }
    }
}
