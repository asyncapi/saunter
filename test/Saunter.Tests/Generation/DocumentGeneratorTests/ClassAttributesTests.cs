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
            var document = documentGenerator.GenerateDocument(new []{ typeof(TenantMessageConsumer).GetTypeInfo() });
            
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
        public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannelInTheSameMethod()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new SchemaGenerator(Options.Create(options));
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator);
            
            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(TenantMessagePublisher).GetTypeInfo() });

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

            var messages = publish.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);
            
            messages.OneOf.ShouldContain(m => m.Name == "anyTenantCreated");
            messages.OneOf.ShouldContain(m => m.Name == "anyTenantUpdated");
            messages.OneOf.ShouldContain(m => m.Name == "anyTenantRemoved");
        }

        [Fact]
        public void GenerateDocument_GeneratesDocumentWithSingleMessage()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new SchemaGenerator(Options.Create(options));
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator);
            
            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(TenantSingleMessagePublisher).GetTypeInfo() });

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.tenants_history");
            channel.Value.Description.ShouldBe("Tenant events.");
            
            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantSingleMessagePublisher");
            publish.Summary.ShouldBe("Publish single domain event about tenants.");

            var message = publish.Message.ShouldBeOfType<Message>();
            message.Name.ShouldBe("anyTenantCreated");
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [SubscribeOperation(OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.")]
        public class TenantMessageConsumer : ITenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) {}

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) {}

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) {}
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public class TenantMessagePublisher : ITenantMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            [Message(typeof(AnyTenantUpdated))]
            [Message(typeof(AnyTenantRemoved))]
            public void PublishTenantEvent<TEvent>(Guid tenantId, TEvent @event)
                where TEvent : IEvent
            {
            }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantSingleMessagePublisher", Summary = "Publish single domain event about tenants.")]
        public class TenantSingleMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            public void PublishTenantCreated(Guid tenantId, AnyTenantCreated @event)
            {
            }
        }
    }
    
    public class TenantCreated {}
    
    public class TenantUpdated {}
    
    public class TenantRemoved {}
    
    public interface ITenantMessageConsumer
    {
        void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt);
        void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt);
        void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt);
    }
}