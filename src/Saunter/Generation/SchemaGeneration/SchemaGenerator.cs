using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Utils;

namespace Saunter.Generation.SchemaGeneration
{
    public class SchemaGenerator : ISchemaGenerator
    {
        private readonly AsyncApiOptions _options;

        public SchemaGenerator(IOptions<AsyncApiOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public ISchema GenerateSchema(Type type, ISchemaRepository schemaRepository)
        {
            var schemaId = _options.SchemaIdSelector(type);

            var reference = schemaRepository.GetOrAdd(type, schemaId, () => TypeSchemaFactory(type, schemaRepository));

            return reference;
        }

        private Schema TypeSchemaFactory(Type type, ISchemaRepository schemaRepository)
        {
            var schema = GetSchemaIfPrimitive(type);
            if (schema != null)
            {
                return schema;
            }

            schema = GetSchemaIfEnumerable(type, schemaRepository);
            if (schema != null)
            {
                return schema;
            }

            schema = GetSchemaIfDiscriminator(type, schemaRepository);
            if (schema != null)
            {
                return schema;
            }

            var filter = _options.PropertyFilter ?? (_ => true);

            var propertyAndFieldMembers = type.GetProperties()
                .Where(filter)
                .Concat(type.GetFields()).ToArray();

            return CreateSchemaFromPropertyAndFieldMembers(schemaRepository, propertyAndFieldMembers);
        }

        private Schema CreateSchemaFromPropertyAndFieldMembers(ISchemaRepository schemaRepository, MemberInfo[] propertyAndFieldMembers)
        {
            var requiredMembers = new HashSet<string>();
            var schema = new Schema
            {
                Properties = new Dictionary<string, ISchema>()
            };

            var memberNameSelector = _options.PropertyNameSelector ?? ((prop) => prop.Name);
            foreach (var member in propertyAndFieldMembers)
            {
                var underlyingTypeOfMember = Reflection.GetUnderlyingType(member);
                var memberName = memberNameSelector(member);

                ISchema memberSchema = GetSchemaIfPrimitive(underlyingTypeOfMember);

                if (memberSchema == null)
                {
                    memberSchema = GetSchemaIfEnumerable(underlyingTypeOfMember, schemaRepository);
                    if (memberSchema != null && memberSchema is Schema s1) // todo: this better
                    {
                        s1.MinItems = member.GetMinItems();
                        s1.MaxItems = member.GetMaxItems();
                        s1.UniqueItems = member.GetIsUniqueItems();
                    }

                    if (memberSchema == null)
                    {
                        memberSchema = GenerateSchema(underlyingTypeOfMember, schemaRepository);
                    }
                }

                if (memberSchema is Schema s2) // todo: this means we won't get anything on reference types.... is this okay???
                {
                    s2.Title = member.GetTitle();
                    s2.Description = member.GetDescription();
                    s2.Minimum = member.GetMinimum();
                    s2.Maximum = member.GetMaximum();
                    s2.MinLength = member.GetMinLength();
                    s2.MaxLength = member.GetMaxLength();
                    s2.Pattern = member.GetPattern();
                    s2.Example = member.GetExample();
                }

                if (member.GetIsRequired())
                {
                    requiredMembers.Add(memberName);
                }

                schema.Properties.Add(memberName, memberSchema);
            }

            if (requiredMembers.Count > 0)
            {
                schema.Required = requiredMembers;
            }

            return schema;
        }

        private Schema GetSchemaIfPrimitive(Type type)
        {
            if (type.IsInteger())
            {
                return new Schema { Type = "integer" };
            }

            if (type.IsNumber())
            {
                return new Schema { Type = "number" };
            }

            if (type == typeof(string))
            {
                return new Schema { Type = "string" };
            }

            if (type.IsBoolean())
            {
                return new Schema { Type = "boolean" };
            }

            if (type.IsEnum(_options, out EnumMembers members))
            {
                return new Schema
                {
                    Type = members.MemberType == typeof(string)
                        ? "string"
                        : "integer",
                    Enum = members.Members,
                };
            }

            if (type.IsDateTime())
            {
                return new Schema
                {
                    Type = "string",
                    Format = "date-time",
                };
            }

            if (type.IsTimeSpan())
            {
                return new Schema
                {
                    Type = "string",
                    Format = "time-span"
                };
            }

            if (type.IsGuid())
            {
                return new Schema
                {
                    Type = "string",
                    Format = "uuid",
                };
            }

            return null;
        }

        private Schema GetSchemaIfEnumerable(Type type, ISchemaRepository schemaRepository)
        {
            if (type.IsEnumerable(out Type elementType))
            {
                return new Schema
                {
                    Type = "array",
                    Items = GenerateSchema(elementType, schemaRepository),
                };
            }

            return null;
        }

        private Schema GetSchemaIfDiscriminator(Type type, ISchemaRepository schemaRepository)
        {
            const bool InheritDiscriminatorAttributes = false;
            var discriminatorAttribute = type.GetCustomAttribute<DiscriminatorAttribute>(InheritDiscriminatorAttributes);
            var discriminatorSubTypeAttributes = type.GetCustomAttributes<DiscriminatorSubTypeAttribute>();
            if (discriminatorAttribute == null || !discriminatorSubTypeAttributes.Any())
            {
                return null;
            }

            return new Schema
            {
                Required = new HashSet<string> { discriminatorAttribute.PropertyName },
                Discriminator = discriminatorAttribute.PropertyName,
                OneOf = discriminatorSubTypeAttributes.Select(x => GenerateSchema(x.SubType, schemaRepository)).ToList(),
            };
        }
    }
}