using System;
using System.Linq;
using System.Reflection;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;
using AsyncApi.Net.Generator.Attributes;
using AsyncApi.Net.Generator.Generation;

using Shouldly;

using Xunit;

namespace AsyncApi.Net.Generator.Tests.Generation.DocumentGeneratorTests;

public class MethodAttributesTests
{
    [Fact]
    public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
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
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { typeof(TenantMessagePublisher).GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

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

    public class TenantMessagePublisher : ITenantMessagePublisher
    {
        [PublishOperation<AnyTenantCreated, AnyTenantUpdated, AnyTenantRemoved>("asw.tenant_service.tenants_history", OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.", ChannelDescription = "Tenant events.")]
        public void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
            where TEvent : IEvent
        {
        }
    }
}

public class AnyTenantCreated : IEvent { }

public class AnyTenantUpdated : IEvent { }

public class AnyTenantRemoved : IEvent { }

public interface IEvent { }

public interface ITenantMessagePublisher
{
    void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
        where TEvent : IEvent;
}