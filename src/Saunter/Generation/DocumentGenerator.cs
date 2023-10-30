using Microsoft.Extensions.DependencyInjection;

using Namotion.Reflection;

using NJsonSchema.Generation;

using Saunter.AsyncApiSchema.v2;
using Saunter.AsyncApiSchema.v2.Bindings;
using Saunter.Attributes;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;
using Saunter.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Saunter.Generation;

public class DocumentGenerator : IDocumentGenerator
{
    public AsyncApiDocument GenerateDocument(TypeInfo[] asyncApiTypes, AsyncApiOptions options, AsyncApiDocument prototype, IServiceProvider serviceProvider)
    {
        AsyncApiDocument asyncApiSchema = prototype.Clone();

        AsyncApiSchemaResolver schemaResolver = new(asyncApiSchema, options.SchemaOptions);

        JsonSchemaGenerator generator = new(options.SchemaOptions);
        asyncApiSchema.Channels = GenerateChannels(asyncApiTypes, schemaResolver, options, generator, serviceProvider);

        DocumentFilterContext filterContext = new(asyncApiTypes, schemaResolver, generator);
        foreach (Type filterType in options.DocumentFilters)
        {
            IDocumentFilter filter = (IDocumentFilter)serviceProvider.GetRequiredService(filterType);
            filter?.Apply(asyncApiSchema, filterContext);
        }

        return asyncApiSchema;
    }

    /// <summary>
    /// Generate the Channels section of an AsyncApi schema.
    /// </summary>
    private static Dictionary<string, ChannelItem> GenerateChannels(TypeInfo[] asyncApiTypes, AsyncApiSchemaResolver schemaResolver, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
    {
        Dictionary<string, ChannelItem> channels = new();

        channels.AddRange(GenerateChannelsFromMethods(asyncApiTypes, schemaResolver, options, jsonSchemaGenerator, serviceProvider));
        channels.AddRange(GenerateChannelsFromClasses(asyncApiTypes, schemaResolver, options, jsonSchemaGenerator, serviceProvider));

        return channels;
    }

    /// <summary>
    /// Generate the Channels section of the AsyncApi schema from the
    /// <see cref="ChannelAttribute"/> on methods.
    /// </summary>
    private static IDictionary<string, ChannelItem> GenerateChannelsFromMethods(IEnumerable<TypeInfo> asyncApiTypes, AsyncApiSchemaResolver schemaResolver, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
    {
        Dictionary<string, ChannelItem> channels = new();

        var methodsWithChannelAttribute = asyncApiTypes
            .SelectMany(type => type.DeclaredMethods)
            .Select(method => new
            {
                Channel = method.GetCustomAttribute<ChannelAttribute>(),
                Method = method,
            })
            .Where(mc => mc.Channel != null);

        foreach (var mc in methodsWithChannelAttribute)
        {
            if (mc.Channel == null)
            {
                continue;
            }

            IEnumerable<Operation> pubs = GenerateOperationFromMethod(
                mc.Method,
                schemaResolver,
                OperationType.Publish,
                options,
                jsonSchemaGenerator,
                serviceProvider);

            foreach (Operation pub in pubs)
            {
                ChannelItem channelItem = new()
                {
                    Description = mc.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(mc.Method, schemaResolver, jsonSchemaGenerator),
                    Publish = pub,
                    Subscribe = null,
                    Bindings = mc.Channel.BindingsRef != null ? new ChannelBindingsReference(mc.Channel.BindingsRef) : null,
                    Servers = mc.Channel.Servers?.ToList() ?? new List<string>(),
                };

                channels.AddOrAppend(mc.Channel.Name, channelItem);

                ChannelItemFilterContext context = new(mc.Method, schemaResolver, jsonSchemaGenerator, mc.Channel);
                foreach (Type filterType in options.ChannelItemFilters)
                {
                    IChannelItemFilter filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                    filter.Apply(channelItem, context);
                }
            }

            IEnumerable<Operation> subs = GenerateOperationFromMethod(
                mc.Method,
                schemaResolver,
                OperationType.Subscribe,
                options,
                jsonSchemaGenerator,
                serviceProvider);

            foreach (Operation sub in subs)
            {
                ChannelItem channelItem = new()
                {
                    Description = mc.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(mc.Method, schemaResolver, jsonSchemaGenerator),
                    Publish = null,
                    Subscribe = sub,
                    Bindings = mc.Channel.BindingsRef != null ? new ChannelBindingsReference(mc.Channel.BindingsRef) : null,
                    Servers = mc.Channel.Servers?.ToList() ?? new List<string>(),
                };

                channels.AddOrAppend(mc.Channel.Name, channelItem);

                ChannelItemFilterContext context = new(mc.Method, schemaResolver, jsonSchemaGenerator, mc.Channel);
                foreach (Type filterType in options.ChannelItemFilters)
                {
                    IChannelItemFilter filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                    filter.Apply(channelItem, context);
                }
            }
        }

        return channels;
    }

    /// <summary>
    /// Generate the Channels section of the AsyncApi schema from the
    /// <see cref="ChannelAttribute"/> on classes.
    /// </summary>
    private static IDictionary<string, ChannelItem> GenerateChannelsFromClasses(IEnumerable<TypeInfo> asyncApiTypes, AsyncApiSchemaResolver schemaResolver, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
    {
        Dictionary<string, ChannelItem> channels = new();

        var classesWithChannelAttribute = asyncApiTypes
            .Select(type => new
            {
                Channel = type.GetCustomAttribute<ChannelAttribute>(),
                Type = type,
            })
            .Where(cc => cc.Channel != null);

        foreach (var cc in classesWithChannelAttribute)
        {
            if (cc.Channel == null)
            {
                continue;
            }

            IEnumerable<Operation> pubs = GenerateOperationFromClass(cc.Type, schemaResolver, OperationType.Publish, jsonSchemaGenerator);

            foreach (Operation item in pubs)
            {
                ChannelItem channelItem = new()
                {
                    Description = cc.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(cc.Type, schemaResolver, jsonSchemaGenerator),
                    Publish = item,
                    Subscribe = null,
                    Bindings = cc.Channel.BindingsRef != null ? new ChannelBindingsReference(cc.Channel.BindingsRef) : null,
                    Servers = cc.Channel.Servers?.ToList() ?? new(),
                };

                channels.AddOrAppend(cc.Channel.Name, channelItem);

                ChannelItemFilterContext context = new(cc.Type, schemaResolver, jsonSchemaGenerator, cc.Channel);
                foreach (Type filterType in options.ChannelItemFilters)
                {
                    IChannelItemFilter filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                    filter.Apply(channelItem, context);
                }
            }

            IEnumerable<Operation> subs = GenerateOperationFromClass(cc.Type, schemaResolver, OperationType.Subscribe, jsonSchemaGenerator);

            foreach (Operation item in subs)
            {
                ChannelItem channelItem = new()
                {
                    Description = cc.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(cc.Type, schemaResolver, jsonSchemaGenerator),
                    Publish = null,
                    Subscribe = item,
                    Bindings = cc.Channel.BindingsRef != null ? new ChannelBindingsReference(cc.Channel.BindingsRef) : null,
                    Servers = cc.Channel.Servers?.ToList() ?? new(),
                };

                channels.AddOrAppend(cc.Channel.Name, channelItem);

                ChannelItemFilterContext context = new(cc.Type, schemaResolver, jsonSchemaGenerator, cc.Channel);
                foreach (Type filterType in options.ChannelItemFilters)
                {
                    IChannelItemFilter filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                    filter.Apply(channelItem, context);
                }
            }
        }

        return channels;
    }

    /// <summary>
    /// Generate the an operation of an AsyncApi Channel for the given method.
    /// </summary>
    private static IEnumerable<Operation> GenerateOperationFromMethod(MethodInfo method, AsyncApiSchemaResolver schemaResolver, OperationType operationType, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
    {
        IEnumerable<OperationAttribute> operationAttributes = GetOperationAttribute(method, operationType);

        if (!operationAttributes.Any())
        {
            yield break;
        }

        IEnumerable<MessageAttribute> messageAttributes = method.GetCustomAttributes<MessageAttribute>();

        foreach (OperationAttribute operationAttribute in operationAttributes)
        {
            IMessage? message = messageAttributes.Any()
                ? GenerateMessageFromAttributes(messageAttributes, schemaResolver, jsonSchemaGenerator)
                : GenerateMessageFromType(operationAttribute.MessagePayloadType, schemaResolver, jsonSchemaGenerator);

            Operation operation = new()
            {
                OperationId = operationAttribute.OperationId ?? method.Name,
                Summary = operationAttribute.Summary ?? method.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (method.GetXmlDocsRemarks() != string.Empty ? method.GetXmlDocsRemarks() : string.Empty),
                Message = message!,
                Bindings = operationAttribute.BindingsRef != null ? new OperationBindingsReference(operationAttribute.BindingsRef) : null,
                Tags = new(operationAttribute.Tags?.Select(x => (Tag)x) ?? new List<Tag>())
            };

            OperationFilterContext filterContext = new(method, schemaResolver, jsonSchemaGenerator, operationAttribute);

            foreach (Type filterType in options.OperationFilters)
            {
                IOperationFilter filter = (IOperationFilter)serviceProvider.GetRequiredService(filterType);
                filter?.Apply(operation, filterContext);
            }

            yield return operation;
        }
    }

    /// <summary>
    /// Generate the an operation of an AsyncApi Channel for the given class.
    /// </summary>
    private static IEnumerable<Operation> GenerateOperationFromClass(TypeInfo type, AsyncApiSchemaResolver schemaResolver, OperationType operationType, JsonSchemaGenerator jsonSchemaGenerator)
    {
        IEnumerable<OperationAttribute> operationAttributes = GetOperationAttribute(type, operationType);

        if (!operationAttributes.Any())
        {
            yield break;
        }

        foreach (var operationAttribute in operationAttributes)
        {
            Messages messages = new();
            Operation operation = new()
            {
                OperationId = operationAttribute.OperationId ?? type.Name,
                Summary = operationAttribute.Summary ?? type.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (type.GetXmlDocsRemarks() != string.Empty ? type.GetXmlDocsRemarks() : string.Empty),
                Message = messages,
                Bindings = operationAttribute.BindingsRef != null ? new OperationBindingsReference(operationAttribute.BindingsRef) : null,
                Tags = new HashSet<Tag>(operationAttribute.Tags?.Select(x => (Tag)x) ?? new List<Tag>())
            };

            var methodsWithMessageAttribute = type.DeclaredMethods
                .Select(method => new
                {
                    MessageAttributes = method.GetCustomAttributes<MessageAttribute>(),
                    Method = method,
                })
                .Where(mm => mm.MessageAttributes.Any());

            foreach (MessageAttribute messageAttribute in methodsWithMessageAttribute.SelectMany(x => x.MessageAttributes))
            {
                IMessage? message = GenerateMessageFromAttribute(messageAttribute, schemaResolver, jsonSchemaGenerator);
                if (message != null)
                {
                    messages.OneOf.Add(message);
                }
            }

            if (messages.OneOf.Count == 1)
            {
                operation.Message = messages.OneOf[0];
            }

            yield return operation;
        }
    }

    private static IEnumerable<OperationAttribute> GetOperationAttribute(MemberInfo typeOrMethod, OperationType operationType)
    {
        return operationType switch
        {
            OperationType.Publish => typeOrMethod.GetCustomAttributes<PublishOperationAttribute>(),
            OperationType.Subscribe => typeOrMethod.GetCustomAttributes<SubscribeOperationAttribute>(),
            _ => Enumerable.Empty<OperationAttribute>(),
        };
    }

    private static IMessage? GenerateMessageFromAttributes(IEnumerable<MessageAttribute> messageAttributes, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        if (messageAttributes.Count() == 1)
        {
            return GenerateMessageFromAttribute(messageAttributes.First(), schemaResolver, jsonSchemaGenerator);
        }

        Messages messages = new();
        foreach (MessageAttribute messageAttribute in messageAttributes)
        {
            IMessage? message = GenerateMessageFromAttribute(messageAttribute, schemaResolver, jsonSchemaGenerator);
            if (message != null)
            {
                messages.OneOf.Add(message);
            }
        }

        if (messages.OneOf.Count == 1)
        {
            return messages.OneOf[0];
        }

        return messages;
    }

    private static IMessage? GenerateMessageFromAttribute(MessageAttribute messageAttribute, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        if (messageAttribute?.PayloadType == null)
        {
            return null;
        }

        Message message = new()
        {
            MessageId = messageAttribute.MessageId!,
            Payload = jsonSchemaGenerator.Generate(messageAttribute.PayloadType, schemaResolver),
            Headers = messageAttribute.HeadersType != null ? jsonSchemaGenerator.Generate(messageAttribute.HeadersType, schemaResolver) : null,
            Title = messageAttribute.Title,
            Summary = messageAttribute.Summary,
            Description = messageAttribute.Description,
            Bindings = messageAttribute.BindingsRef != null ? new MessageBindingsReference(messageAttribute.BindingsRef) : null,
            Tags = new HashSet<Tag>(messageAttribute.Tags?.Select(x => (Tag)x) ?? new List<Tag>())
        };
        message.Name = messageAttribute.Name ?? message.Payload.ActualSchema.Id;

        return schemaResolver.GetMessageOrReference(message);
    }


    private static IMessage? GenerateMessageFromType(Type? payloadType, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        if (payloadType == null)
        {
            return null;
        }

        Message message = new()
        {
            Payload = jsonSchemaGenerator.Generate(payloadType, schemaResolver),
        };

        message.Name = message.Payload.Id;

        return schemaResolver.GetMessageOrReference(message);
    }

    private static Dictionary<string, IParameter> GetChannelParametersFromAttributes(MemberInfo memberInfo, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        IEnumerable<ChannelParameterAttribute> attributes = memberInfo.GetCustomAttributes<ChannelParameterAttribute>();
        Dictionary<string, IParameter> parameters = new();
        if (attributes.Any())
        {
            foreach (ChannelParameterAttribute attribute in attributes)
            {
                IParameter parameter = schemaResolver.GetParameterOrReference(new Parameter
                {
                    Description = attribute.Description,
                    Name = attribute.Name,
                    Schema = jsonSchemaGenerator.Generate(attribute.Type, schemaResolver),
                    Location = attribute.Location,
                });

                parameters.Add(attribute.Name, parameter);
            }
        }

        return parameters;
    }
}