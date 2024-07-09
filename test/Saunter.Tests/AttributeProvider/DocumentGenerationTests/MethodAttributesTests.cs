using System;
using System.Linq;
using System.Reflection;
using Saunter.AttributeProvider.Attributes;
using Saunter.Options;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.DocumentGeneratorTests
{
    public class MethodAttributesTests
    {
        [Fact]
        public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, typeof(TenantMessagePublisher));

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");

            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");

            publish.Message.Count.ShouldBe(3);

            publish.Message.ShouldContain(m => m.MessageId == "anyTenantCreated");
            publish.Message.ShouldContain(m => m.MessageId == "anyTenantUpdated");
            publish.Message.ShouldContain(m => m.MessageId == "anyTenantRemoved");
        }

        [AsyncApi]
        public class TenantMessagePublisher : ITenantMessagePublisher
        {
            [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
            [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
            [Message(typeof(AnyTenantCreated))]
            [Message(typeof(AnyTenantUpdated))]
            [Message(typeof(AnyTenantRemoved))]
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
}
