using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using LEGO.AsyncAPI.Models;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.SharedKernel
{
    internal class AsyncApiSchemaGenerator : IAsyncApiSchemaGenerator
    {
        public AsyncApiSchema? Generate(Type? type)
        {
            if (type is null)
            {
                return null;
            }

            var typeInfo = type.GetTypeInfo();

            var name = typeInfo.Name;
            name = char.ToLowerInvariant(name[0]) + name[1..];

            var schema = new AsyncApiSchema
            {
                Title = name,
                Type = MapJsonTypeToSchemaType(typeInfo),
            };

            if (schema.Type is not SchemaType.Object and not SchemaType.Array)
            {
                return schema;
            }

            schema.Properties = typeInfo
                .DeclaredProperties
                .Where(p => p.GetMethod is not null && !p.GetMethod.IsStatic)
                .ToDictionary(
                    prop => prop.Name,
                    prop => Generate(prop.PropertyType.GetTypeInfo()));

            return schema;
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
