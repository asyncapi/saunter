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
        public void GenerateDocument_GeneratesDocumentWithChannelParameters()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var schemaGenerator = new SchemaGenerator(Options.Create(options));
            var documentGenerator = new DocumentGenerator(Options.Create(options), schemaGenerator);
            
            // Act
            var document = documentGenerator.GenerateDocument(new []{ typeof(OneTenantMessageConsumer).GetTypeInfo() });
            
            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.{tenant_id}.{tenant_status}");
            channel.Value.Description.ShouldBe("A tenant events.");
            channel.Value.Parameters.Count.ShouldBe(2);
            channel.Value.Parameters.ShouldContain(p => p.Key.ToString() == "tenant_id" && p.Value.Schema != null && p.Value.Description == "The tenant identifier.");
            channel.Value.Parameters.ShouldContain(p => p.Key.ToString() == "tenant_status" && p.Value.Schema != null && p.Value.Description == "The tenant status.");
            
            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("OneTenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about a tenant.");

            var messages = subscribe.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);
            
            messages.OneOf.ShouldContain(m => m.Name == "tenantCreated");
            messages.OneOf.ShouldContain(m => m.Name == "tenantUpdated");
            messages.OneOf.ShouldContain(m => m.Name == "tenantRemoved");
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
        [Channel("asw.tenant_service.{tenant_id}.{tenant_status}", Description = "A tenant events.")]
        [ChannelParameter("tenant_id", typeof(long), Description = "The tenant identifier.")]
        [ChannelParameter("tenant_status", typeof(string), Description = "The tenant status.")]
        [SubscribeOperation(OperationId = "OneTenantMessageConsumer", Summary = "Subscribe to domains events about a tenant.")]
        public class OneTenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid tenantId, TenantCreated evnt) {}

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid tenantId, TenantUpdated evnt) {}

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid tenantId, TenantRemoved evnt) {}
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