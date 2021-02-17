using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation;
using Saunter.Generation.SchemaGeneration;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.DocumentGeneratorTests
{
    public class ClassAttributesTests
    {
        [Fact]
        public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new SchemaGenerator(Options.Create(options));
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator);

            // Act
            var document = documentGenerator.GenerateDocument(new[] { typeof(TenantMessageConsumer).GetTypeInfo() });

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");

            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("TenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

            var messages = subscribe.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);

            messages.OneOf.ShouldContain(m => m.Name == "tenantCreated");
            messages.OneOf.ShouldContain(m => m.Name == "tenantUpdated");
            messages.OneOf.ShouldContain(m => m.Name == "tenantRemoved");
        }

        [Fact]
        public void GetDocument_WhenMultipleClassesUseSameChannelKey_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new SchemaGenerator(Options.Create(options));
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator);

            // Act
            var document = documentGenerator.GenerateDocument(new[]
            {
                typeof(TenantMessageConsumer).GetTypeInfo(),
                typeof(TenantMessagePublisher).GetTypeInfo()
            });

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");

            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("TenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");

            var subscribeMessages = subscribe.Message.ShouldBeOfType<Messages>();
            subscribeMessages.OneOf.Count.ShouldBe(3);

            subscribeMessages.OneOf.ShouldContain(m => m.Name == "tenantCreated");
            subscribeMessages.OneOf.ShouldContain(m => m.Name == "tenantUpdated");
            subscribeMessages.OneOf.ShouldContain(m => m.Name == "tenantRemoved");

            var publishMessages = subscribe.Message.ShouldBeOfType<Messages>();
            publishMessages.OneOf.Count.ShouldBe(3);

            publishMessages.OneOf.ShouldContain(m => m.Name == "tenantCreated");
            publishMessages.OneOf.ShouldContain(m => m.Name == "tenantUpdated");
            publishMessages.OneOf.ShouldContain(m => m.Name == "tenantRemoved");
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [SubscribeOperation(OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.")]
        public class TenantMessageConsumer : ITenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public class TenantMessagePublisher : ITenantMessagePublisher
        {
            [Message(typeof(TenantCreated))]
            public void PublishTenantCreatedEvent(Guid tenantId, TenantCreated evnt) { }

            [Message(typeof(TenantUpdated))]
            public void PublishTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) { }

            [Message(typeof(TenantRemoved))]
            public void PublishTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) { }
        }
    }

    public class TenantCreated { }

    public class TenantUpdated { }

    public class TenantRemoved { }

    public interface ITenantMessageConsumer
    {
        void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt);

        void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt);

        void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt);
    }

    public interface ITenantMessagePublisher
    {
        void PublishTenantCreatedEvent(Guid tenantId, TenantCreated evnt);

        void PublishTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt);

        void PublishTenantRemovedEvent(Guid tenantId, TenantRemoved evnt);
    }
}