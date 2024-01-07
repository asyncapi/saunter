using Microsoft.Extensions.DependencyInjection;

using Namotion.Reflection;

using NJsonSchema.Generation;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;
using AsyncApi.Net.Generator.Attributes;
using AsyncApi.Net.Generator.Generation.Filters;
using AsyncApi.Net.Generator.Generation.SchemaGeneration;
using AsyncApi.Net.Generator.Utils;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AsyncApi.Net.Generator.Generation;

public class DocumentGenerator : IDocumentGenerator
{
    public AsyncApiDocument GenerateDocument(
        TypeInfo[] assemblyTypes,
        AsyncApiOptions options,
        AsyncApiDocument prototype,
        IServiceProvider serviceProvider)
    {
        AsyncApiDocument asyncApiSchema = prototype.Clone();

        AsyncApiSchemaResolver schemaResolver = new(asyncApiSchema, options.SchemaOptions);

        JsonSchemaGenerator generator = new(options.SchemaOptions);

        ConcurrentDictionary<TypeInfo, IMessage> messageMap = SelectAssemblyMessages(
            assemblyTypes,
            schemaResolver,
            generator);

        asyncApiSchema.Channels = GenerateChannels(
            asyncApiSchema.DocumentName,
            assemblyTypes,
            schemaResolver,
            options,
            generator,
            messageMap,
            serviceProvider);

        DocumentFilterContext filterContext = new(assemblyTypes, schemaResolver, generator);

        foreach (Type filterType in options.DocumentFilters)
        {
            IDocumentFilter filter = (IDocumentFilter)serviceProvider.GetRequiredService(filterType);
            filter?.Apply(asyncApiSchema, filterContext);
        }

        return asyncApiSchema;
    }

    private static ConcurrentDictionary<TypeInfo, IMessage> SelectAssemblyMessages(TypeInfo[] asyncApiTypes, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        ConcurrentDictionary<TypeInfo, IMessage> map = new();

        foreach (TypeInfo payload in asyncApiTypes)
        {
            MessageAttribute? attribute = payload.GetCustomAttribute<MessageAttribute>();
            if (attribute is null)
            {
                continue;
            }

            map[payload] = (GenerateMessageFromAttribute(
                payload,
                attribute,
                schemaResolver,
                jsonSchemaGenerator));
        }

        return map;
    }

    /// <summary>
    /// Generate the Channels section of an AsyncApi schema.
    /// </summary>
    private static Dictionary<string, Channel> GenerateChannels(
        string? documentName,
        TypeInfo[] asyncApiTypes,
        AsyncApiSchemaResolver schemaResolver,
        AsyncApiOptions options,
        JsonSchemaGenerator jsonSchemaGenerator,
        ConcurrentDictionary<TypeInfo, IMessage> messageMap,
        IServiceProvider serviceProvider)
    {
        Dictionary<string, Channel> channels = [];

        foreach (TypeInfo type in asyncApiTypes)
        {
            foreach (MethodInfo item in type.DeclaredMethods)
            {
                GenerateChannelsFromMethods(
                    documentName,
                    channels,
                    item,
                    schemaResolver,
                    options,
                    jsonSchemaGenerator,
                    messageMap,
                    serviceProvider);
            }

            GenerateChannelsFromClasses(
                documentName,
                channels,
                type,
                schemaResolver,
                options,
                jsonSchemaGenerator,
                messageMap,
                serviceProvider);
        }

        return channels;
    }

    /// <summary>
    /// Generate the an operation of an AsyncApi Channel for the given method.
    /// </summary>
    private static void GenerateChannelsFromMethods(
        string? documentName,
        Dictionary<string, Channel> channels,
        MethodInfo method,
        AsyncApiSchemaResolver schemaResolver,
        AsyncApiOptions options,
        JsonSchemaGenerator jsonSchemaGenerator,
        ConcurrentDictionary<TypeInfo, IMessage> messageMap,
        IServiceProvider serviceProvider)
    {
        IEnumerable<OperationAttribute> operationAttributes = GetOperationAttribute(method);

        if (!operationAttributes.Any())
        {
            return;
        }

        foreach (OperationAttribute operationAttribute in operationAttributes)
        {
            if (operationAttribute.DocumentName != documentName) { continue; }

            IMessage targetPayload = GenerateOperationMessage(schemaResolver, jsonSchemaGenerator, messageMap, operationAttribute);

            Operation operation = new()
            {
                OperationId = operationAttribute.OperationId ??
                    (operationAttribute.ChannelName + '/' + string.Join('|',operationAttribute.MessagePayloadTypes.Select(t => t.Name))),
                Summary = operationAttribute.Summary ?? method.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (method.GetXmlDocsRemarks() != string.Empty ? method.GetXmlDocsRemarks() : string.Empty),
                Message = targetPayload,
                Bindings = operationAttribute.BindingsRef != null ? new OperationBindingsReference(operationAttribute.BindingsRef) : null,
                Tags = new(operationAttribute.Tags?.Select(x => (Tag)x) ?? new List<Tag>())
            };

            Channel channelItem = new()
            {
                Servers = operationAttribute.ChannelServers?.ToList(),
                Description = operationAttribute.ChannelDescription,
                Parameters = GetChannelParametersFromAttributes(operationAttribute.ChannelName, schemaResolver, jsonSchemaGenerator),
            };

            if (operationAttribute.BindingsRef is not null)
            {
                channelItem.Bindings = new ChannelBindingsReference(operationAttribute.BindingsRef);
            }

            if (operationAttribute.OperationType is OperationType.Publish)
            {
                channelItem.Publish = operation;
            }
            else if (operationAttribute.OperationType is OperationType.Subscribe)
            {
                channelItem.Subscribe = operation;
            }

            OperationFilterContext filterContext = new(method, schemaResolver, jsonSchemaGenerator, operationAttribute);

            foreach (Type filterType in options.OperationFilters)
            {
                IOperationFilter filter = (IOperationFilter)serviceProvider.GetRequiredService(filterType);
                filter?.Apply(operation, filterContext);
            }

            ChannelItemFilterContext context = new(method, schemaResolver, jsonSchemaGenerator);
            foreach (Type filterType in options.ChannelItemFilters)
            {
                IChannelItemFilter filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                filter.Apply(channelItem, context);
            }

            channels.AddOrAppend(operationAttribute.ChannelName, channelItem);
        }
    }

    /// <summary>
    /// Generate the an operation of an AsyncApi Channel for the given class.
    /// </summary>
    private static void GenerateChannelsFromClasses(
        string? documentName,
        Dictionary<string, Channel> channels,
        TypeInfo type,
        AsyncApiSchemaResolver schemaResolver,
        AsyncApiOptions options,
        JsonSchemaGenerator jsonSchemaGenerator,
        ConcurrentDictionary<TypeInfo, IMessage> messageMap,
        IServiceProvider serviceProvider)
    {
        IEnumerable<OperationAttribute> operationAttributes = GetOperationAttribute(type);

        if (!operationAttributes.Any())
        {
            return;
        }

        foreach (OperationAttribute operationAttribute in operationAttributes)
        {
            if (operationAttribute.DocumentName != documentName) { continue; }

            IMessage targetPayload = GenerateOperationMessage(schemaResolver, jsonSchemaGenerator, messageMap, operationAttribute);

            Operation operation = new()
            {
                OperationId = operationAttribute.OperationId ??
                    (operationAttribute.ChannelName + '/' + string.Join('|', operationAttribute.MessagePayloadTypes.Select(t => t.Name))),
                Summary = operationAttribute.Summary ?? type.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (type.GetXmlDocsRemarks() != string.Empty ? type.GetXmlDocsRemarks() : string.Empty),
                Message = targetPayload,
                Bindings = operationAttribute.BindingsRef != null ? new OperationBindingsReference(operationAttribute.BindingsRef) : null,
                Tags = new(operationAttribute.Tags?.Select(x => (Tag)x) ?? new List<Tag>())
            };

            Channel channelItem = new()
            {
                Servers = operationAttribute.ChannelServers?.ToList(),
                Description = operationAttribute.ChannelDescription,
                Parameters = GetChannelParametersFromAttributes(operationAttribute.ChannelName, schemaResolver, jsonSchemaGenerator),
            };

            if (operationAttribute.BindingsRef is not null)
            {
                channelItem.Bindings = new ChannelBindingsReference(operationAttribute.BindingsRef);
            }

            if (operationAttribute.OperationType is OperationType.Publish)
            {
                channelItem.Publish = operation;
            }
            else if (operationAttribute.OperationType is OperationType.Subscribe)
            {
                channelItem.Subscribe = operation;
            }

            ChannelItemFilterContext context = new(type, schemaResolver, jsonSchemaGenerator);
            foreach (Type filterType in options.ChannelItemFilters)
            {
                IChannelItemFilter filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                filter.Apply(channelItem, context);
            }

            channels.AddOrAppend(operationAttribute.ChannelName, channelItem);
        }

        return;
    }

    private static IEnumerable<OperationAttribute> GetOperationAttribute(MemberInfo typeOrMethod)
    {
        return typeOrMethod
            .GetCustomAttributes<PublishOperationAttribute>()
            .OfType<OperationAttribute>()
            .Concat(typeOrMethod
                .GetCustomAttributes<SubscribeOperationAttribute>());
    }

    private static IMessage GenerateOperationMessage(
        AsyncApiSchemaResolver schemaResolver,
        JsonSchemaGenerator jsonSchemaGenerator,
        ConcurrentDictionary<TypeInfo, IMessage> messageMap,
        OperationAttribute operationAttribute)
    {
        IMessage targetPayload;

        if (operationAttribute.MessagePayloadTypes.Length > 1)
        {
            Messages messages = new();

            foreach (TypeInfo messageType in operationAttribute.MessagePayloadTypes)
            {
                IMessage message = messageMap.GetOrAdd(messageType, p =>
                    GenerateMessageFromType(p, schemaResolver, jsonSchemaGenerator));

                messages.OneOf.Add(message);
            }

            targetPayload = messages;
        }
        else
        {
            TypeInfo type = operationAttribute.MessagePayloadTypes[0];

            targetPayload = messageMap.GetOrAdd(type, p =>
                GenerateMessageFromType(p, schemaResolver, jsonSchemaGenerator));
        }

        return targetPayload;
    }

    private static IMessage GenerateMessageFromAttribute(Type payloadType, MessageAttribute messageAttribute, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        Message message = new()
        {
            MessageId = messageAttribute.MessageId!,
            Payload = jsonSchemaGenerator.Generate(payloadType, schemaResolver),
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

    private static IMessage GenerateMessageFromType(Type payloadType, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        Message message = new()
        {
            Payload = jsonSchemaGenerator.Generate(payloadType, schemaResolver),
        };

        message.Name = message.Payload.Id;

        return schemaResolver.GetMessageOrReference(message);
    }

    private static Dictionary<string, IParameter> GetChannelParametersFromAttributes(string channel, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
    {
        Dictionary<string, IParameter> parameters = [];

        int len = channel.Length;
        for (int i = 0; i < len; i++)
        {
            char rune = channel[i];

            if (rune == '{')
            {
                int indexEnd = channel.IndexOf('}', i);
                string parameter = channel[(i + 1)..indexEnd];

                parameters.Add(parameter, schemaResolver.GetParameterOrReference(new Parameter
                {
                    Name = parameter,
                    Schema = jsonSchemaGenerator.Generate(typeof(string), schemaResolver),
                    Description = null, // TODO: fix me
                    Location = null, // TODO: fix me
                }));
            }
        }

        return parameters;
    }
}