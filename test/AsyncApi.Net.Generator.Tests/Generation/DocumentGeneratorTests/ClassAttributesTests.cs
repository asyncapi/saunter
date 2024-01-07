using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;
using AsyncApi.Net.Generator.Attributes;
using AsyncApi.Net.Generator.Generation;

using Shouldly;

using Xunit;

namespace AsyncApi.Net.Generator.Tests.Generation.DocumentGeneratorTests;

public class ClassAttributesTests
{
    [Fact]
    public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                }
            }
        };
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { typeof(TenantMessageConsumer).GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);

        System.Collections.Generic.KeyValuePair<string, Channel> channel = document.Channels.First();
        channel.Key.ShouldBe("asw.tenant_service.tenants_history");
        channel.Value.Description.ShouldBe("Tenant events.");

        Operation subscribe = channel.Value.Subscribe;
        subscribe.ShouldNotBeNull();
        subscribe.OperationId.ShouldBe("TenantMessageConsumer");
        subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

        Messages messages = subscribe.Message.ShouldBeOfType<Messages>();
        messages.OneOf.Count.ShouldBe(3);

        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
    }


    [Fact]
    public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannelInTheSameMethod()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                }
            }
        };
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { typeof(TenantGenericMessagePublisher).GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);

        System.Collections.Generic.KeyValuePair<string, Channel> channel = document.Channels.First();
        channel.Key.ShouldBe("asw.tenant_service.tenants_history");
        channel.Value.Description.ShouldBe("Tenant events.");

        Operation publish = channel.Value.Publish;
        publish.ShouldNotBeNull();
        publish.OperationId.ShouldBe("TenantMessagePublisher");
        publish.Summary.ShouldBe("Publish domains events about tenants.");

        Messages messages = publish.Message.ShouldBeOfType<Messages>();
        messages.OneOf.Count.ShouldBe(3);

        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantCreated");
        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantUpdated");
        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantRemoved");
    }


    [Fact]
    public void GenerateDocument_GeneratesDocumentWithSingleMessage()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                }
            }
        };
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { typeof(TenantSingleMessagePublisher).GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);

        System.Collections.Generic.KeyValuePair<string, Channel> channel = document.Channels.First();
        channel.Key.ShouldBe("asw.tenant_service.tenants_history");
        channel.Value.Description.ShouldBe("Tenant events.");

        Operation publish = channel.Value.Publish;
        publish.ShouldNotBeNull();
        publish.OperationId.ShouldBe("TenantSingleMessagePublisher");
        publish.Summary.ShouldBe("Publish single domain event about tenants.");

        MessageReference message = publish.Message.ShouldBeOfType<MessageReference>();
        message.Id.ShouldBe("anyTenantCreated");
    }


    [Fact]
    public void GetDocument_WhenMultipleClassesUseSameChannelKey_GeneratesDocumentWithMultipleMessagesPerChannel()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                }
            }
        };
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[]
        {
            typeof(TenantMessageConsumer).GetTypeInfo(),
            typeof(TenantMessagePublisher).GetTypeInfo()
        }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);

        System.Collections.Generic.KeyValuePair<string, Channel> channel = document.Channels.First();
        channel.Key.ShouldBe("asw.tenant_service.tenants_history");
        channel.Value.Description.ShouldBe("Tenant events.");

        Operation subscribe = channel.Value.Subscribe;
        subscribe.ShouldNotBeNull();
        subscribe.OperationId.ShouldBe("TenantMessageConsumer");
        subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

        Operation publish = channel.Value.Publish;
        publish.ShouldNotBeNull();
        publish.OperationId.ShouldBe("TenantMessagePublisher");
        publish.Summary.ShouldBe("Publish domains events about tenants.");


        Messages subscribeMessages = subscribe.Message.ShouldBeOfType<Messages>();
        subscribeMessages.OneOf.Count.ShouldBe(3);

        subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
        subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
        subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");

        Messages publishMessages = subscribe.Message.ShouldBeOfType<Messages>();
        publishMessages.OneOf.Count.ShouldBe(3);

        publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
        publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
        publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
    }


    [Fact]
    public void GenerateDocument_GeneratesDocumentWithChannelParameters()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                },
                Components = new()
                {
                    Parameters = new()
                    {
                        {
                            "tenant_id",
                            new()
                            {
                                Description = "The tenant identifier.",
                                Schema = NJsonSchema.JsonSchema.FromType(typeof(string)),
                                Location = "tester",
                            }
                        }
                    },
                },
            },
        };
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(
            new[] { typeof(OneTenantMessageConsumer).GetTypeInfo() },
            options,
            options.AsyncApi,
            ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);

        System.Collections.Generic.KeyValuePair<string, Channel> channel = document.Channels.First();
        channel.Key.ShouldBe("asw.tenant_service.{tenant_id}.{tenant_status}");
        channel.Value.Description.ShouldBe("A tenant events.");
        channel.Value.Parameters.Count.ShouldBe(2);
        channel.Value.Parameters.Values.OfType<ParameterReference>().ShouldContain(p => p.Id == "tenant_id" && p.Value.Schema != null && p.Value.Description == "The tenant identifier." && p.Value.Location == "tester");
        channel.Value.Parameters.Values.OfType<ParameterReference>().ShouldContain(p => p.Id == "tenant_status" && p.Value.Schema != null && p.Value.Description == null);

        Operation subscribe = channel.Value.Subscribe;
        subscribe.ShouldNotBeNull();
        subscribe.OperationId.ShouldBe("OneTenantMessageConsumer");
        subscribe.Summary.ShouldBe("Subscribe to domains events about a tenant.");

        Messages messages = subscribe.Message.ShouldBeOfType<Messages>();
        messages.OneOf.Count.ShouldBe(3);

        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
        messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
    }


    [Fact]
    public void GenerateDocument_GeneratesDocumentWithMessageHeader()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                }
            }
        };
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(
            new[] { typeof(MyMessagePublisher).GetTypeInfo(), typeof(MyMessage).GetTypeInfo() },
            options,
            options.AsyncApi,
            ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();

        document.Components.Schemas.Values.ShouldContain(t => t.Id == "myMessageHeader");
        Message message = document.Components.Messages.Values.First();
        message.Headers.Reference.Id.ShouldBe("myMessageHeader");
    }

    [PublishOperation<MyMessage>("channel.my.message")]
    public class MyMessagePublisher
    {
        public void PublishMyMessage() { }
    }

    [SubscribeOperation<TenantCreated, TenantUpdated, TenantRemoved>("asw.tenant_service.tenants_history", OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.", ChannelDescription = "Tenant events.")]
    public class TenantMessageConsumer
    {
        public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

        public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

        public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
    }

    [PublishOperation<TenantCreated, TenantUpdated, TenantRemoved>("asw.tenant_service.tenants_history", OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.", ChannelDescription = "Tenant events.")]
    public class TenantMessagePublisher
    {
        public void PublishTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

        public void PublishTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

        public void PublishTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
    }

    [PublishOperation<AnyTenantCreated, AnyTenantUpdated, AnyTenantRemoved>("asw.tenant_service.tenants_history", OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.", ChannelDescription = "Tenant events.")]
    public class TenantGenericMessagePublisher
    {
        public void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
            where TEvent : IEvent
        {
        }
    }

    [PublishOperation<AnyTenantCreated>("asw.tenant_service.tenants_history", OperationId = "TenantSingleMessagePublisher", Summary = "Publish single domain event about tenants.", ChannelDescription = "Tenant events.")]
    public class TenantSingleMessagePublisher
    {
        public void PublishTenantCreated(Guid tenantId, AnyTenantCreated @event)
        {
        }
    }

    [SubscribeOperation<TenantCreated, TenantUpdated, TenantRemoved>("asw.tenant_service.{tenant_id}.{tenant_status}", OperationId = "OneTenantMessageConsumer", Summary = "Subscribe to domains events about a tenant.", ChannelDescription = "A tenant events.")]
    public class OneTenantMessageConsumer
    {
        public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

        public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

        public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
    }
}

public class TenantCreated { }

public class TenantUpdated { }

public class TenantRemoved { }


[Message(HeadersType = typeof(MyMessageHeader))]
public class MyMessage { }

public class MyMessageHeader
{
    [Required]
    public string StringHeader { get; set; }
    public int? NullableIntHeader { get; set; }
}