using System;
using System.Linq;
using Saunter.AttributeProvider.Attributes;
using Shouldly;
using Xunit;

namespace Saunter.Tests.AttributeProvider.DocumentGenerationTests
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

            var channel = document.AssertAndGetChannel("asw.tenant_service.tenants_history", "Tenant events.");

            var publish = channel.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");

            document.AssertByMessage(publish, "anyTenantCreated", "anyTenantUpdated", "anyTenantRemoved");
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
