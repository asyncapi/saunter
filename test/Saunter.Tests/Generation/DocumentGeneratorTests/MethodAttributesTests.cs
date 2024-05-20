using System;
using System.Linq;
using System.Reflection;

using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Generation.DocumentGeneratorTests;

public class MethodAttributesTests
{
    [Fact]
    public void GenerateDocumentGeneratesDocumentWithMultipleMessagesPerChannel()
    {
        // Arrange
        AsyncApiOptions options = new();
        DocumentGenerator documentGenerator = new();

        // Act
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { typeof(TenantMessagePublisher).GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);

        System.Collections.Generic.KeyValuePair<string, ChannelItem> channel = document.Channels.First();
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

    [AsyncApi]
    public class TenantMessagePublisher : ITenantMessagePublisher
    {
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        [Message(typeof(AnyTenantCreated))]
        [Message(typeof(AnyTenantUpdated))]
        [Message(typeof(AnyTenantRemoved))]
        public void PublishTenantEvent<TMessage>(Guid tenantId, TMessage message)
            where TMessage : IEvent
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
    void PublishTenantEvent<TMessage>(Guid tenantId, TMessage message)
        where TMessage : IEvent;
}
