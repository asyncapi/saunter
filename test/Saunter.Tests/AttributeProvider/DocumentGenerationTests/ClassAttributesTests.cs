using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using Saunter.AttributeProvider.Attributes;
using Shouldly;
using Xunit;
using YamlDotNet.Core.Tokens;

namespace Saunter.Tests.AttributeProvider.DocumentGenerationTests
{
    public class ClassAttributesTests
    {
        [Theory]
        [InlineData(typeof(TenantMessageConsumer))]
        [InlineData(typeof(ITenantMessageConsumer))]
        public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel(Type type)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);
            const string Key = "asw.tenant_service.tenants_history";

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();
            var channel = document.AssertAndGetChannel(Key, "Tenant events.");

            var subscribe = channel.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("TenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

            document.AssertByMessage(subscribe, "tenantCreated", "tenantUpdated", "tenantRemoved");
        }

        [Theory]
        [InlineData(typeof(TenantGenericMessagePublisher))]
        [InlineData(typeof(ITenantGenericMessagePublisher))]
        public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannelInTheSameMethod(Type type)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);
            const string Key = "asw.tenant_service.tenants_history";

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();

            var channel = document.AssertAndGetChannel(Key, "Tenant events.");

            var publish = channel.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");

            document.AssertByMessage(publish, "anyTenantCreated", "anyTenantUpdated", "anyTenantRemoved");
        }

        [Theory]
        [InlineData(typeof(TenantSingleMessagePublisher))]
        [InlineData(typeof(ITenantSingleMessagePublisher))]
        public void GenerateDocument_GeneratesDocumentWithSingleMessage(Type type)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);
            const string Key = "asw.tenant_service.tenants_history";

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();

            var channel = document.AssertAndGetChannel(Key, "Tenant events.");

            var publish = channel.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantSingleMessagePublisher");
            publish.Summary.ShouldBe("Publish single domain event about tenants.");

            document.AssertByMessage(publish, "anyTenantCreated");
        }


        [Theory]
        [InlineData(typeof(TenantMessageConsumer), typeof(TenantMessagePublisher))]
        [InlineData(typeof(ITenantMessageConsumer), typeof(ITenantMessagePublisher))]
        public void GetDocument_WhenMultipleClassesUseSameChannelKey_GeneratesDocumentWithMultipleMessagesPerChannel(Type type1, Type type2)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type1, type2);
            const string Key = "asw.tenant_service.tenants_history";

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();

            var channel = document.AssertAndGetChannel(Key, "Tenant events.");

            var subscribe = channel.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("TenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about tenants.");

            var publish = channel.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("TenantMessagePublisher");
            publish.Summary.ShouldBe("Publish domains events about tenants.");

            document.AssertByMessage(subscribe, "tenantCreated", "tenantUpdated", "tenantRemoved");
            document.AssertByMessage(publish, "tenantCreated", "tenantUpdated", "tenantRemoved");
        }

        [Theory]
        [InlineData(typeof(OneTenantMessageConsumer))]
        [InlineData(typeof(IOneTenantMessageConsumer))]
        public void GenerateDocument_GeneratesDocumentWithChannelParameters(Type type)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);
            const string Key = "asw.tenant_service.{tenant_id}.{tenant_status}";

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);
            document.Channels.ShouldContainKey(Key);

            var channel = document.Channels[Key];

            channel.Description.ShouldBe("A tenant events.");

            channel.Parameters.Count.ShouldBe(2);
            channel.Parameters.ContainsKey("tenant_id");
            channel.Parameters.ContainsKey("tenant_status");

            document.Components.Parameters.Count.ShouldBe(2);
            document.Components.Parameters.ShouldContain(p => p.Key == "tenant_id" && p.Value.Schema != null && p.Value.Description == "The tenant identifier.");
            document.Components.Parameters.ShouldContain(p => p.Key == "tenant_status" && p.Value.Schema != null && p.Value.Description == "The tenant status.");

            var subscribe = channel.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("OneTenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about a tenant.");

            document.AssertByMessage(subscribe, "tenantCreated", "tenantUpdated", "tenantRemoved");
        }

        [Theory]
        [InlineData(typeof(MyMessagePublisher))]
        [InlineData(typeof(IMyMessagePublisher))]
        public void GenerateDocument_GeneratesDocumentWithMessageHeader(Type type)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(expected: 1);

            var channel = document.Channels.First().Value;
            var messages = channel.Publish.Message;

            messages.Count.ShouldBe(1);

            var message = messages[0];

            document.Components.Messages.ContainsKey(message.Reference.Id);

            var messageFromRef = document.Components.Messages[message.Reference.Id];

            document.Components.Schemas.ContainsKey(messageFromRef.Payload.Reference.Id);
            document.Components.Schemas.ContainsKey(messageFromRef.Headers.Reference.Id);

            document.Components.Schemas[messageFromRef.Headers.Reference.Id].Title.ShouldBe("myMessageHeader");
        }

        [AsyncApi]
        [Channel("channel.my.message")]
        [PublishOperation]
        public class MyMessagePublisher
        {
            [Message(typeof(MyMessage), HeadersType = typeof(MyMessageHeader))]
            public void PublishMyMessage() { }
        }

        [AsyncApi]
        [Channel("channel.my.message.interface")]
        [PublishOperation]
        public interface IMyMessagePublisher
        {
            [Message(typeof(MyMessage), HeadersType = typeof(MyMessageHeader))]
            void PublishMyMessage();
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [SubscribeOperation(OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.")]
        public class TenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid id, TenantCreated created) { }

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid id, TenantUpdated updated) { }

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid id, TenantRemoved removed) { }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [SubscribeOperation(OperationId = "TenantMessageConsumer", Summary = "Subscribe to domains events about tenants.")]
        public interface ITenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            void SubscribeTenantCreatedEvent(Guid _, TenantCreated __);

            [Message(typeof(TenantUpdated))]
            void SubscribeTenantUpdatedEvent(Guid _, TenantUpdated __);

            [Message(typeof(TenantRemoved))]
            void SubscribeTenantRemovedEvent(Guid _, TenantRemoved __);
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public class TenantMessagePublisher
        {
            [Message(typeof(TenantCreated))]
            public void PublishTenantCreatedEvent(Guid id, TenantCreated created) { }

            [Message(typeof(TenantUpdated))]
            public void PublishTenantUpdatedEvent(Guid id, TenantUpdated updated) { }

            [Message(typeof(TenantRemoved))]
            public void PublishTenantRemovedEvent(Guid id, TenantRemoved removed) { }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public interface ITenantMessagePublisher
        {
            [Message(typeof(TenantCreated))]
            void PublishTenantCreatedEvent(Guid _, TenantCreated __);

            [Message(typeof(TenantUpdated))]
            void PublishTenantUpdatedEvent(Guid _, TenantUpdated __);

            [Message(typeof(TenantRemoved))]
            void PublishTenantRemovedEvent(Guid _, TenantRemoved __);
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public class TenantGenericMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            [Message(typeof(AnyTenantUpdated))]
            [Message(typeof(AnyTenantRemoved))]
            public void PublishTenantEvent<TEvent>(Guid id, TEvent @event)
                where TEvent : IEvent
            {
            }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantMessagePublisher", Summary = "Publish domains events about tenants.")]
        public interface ITenantGenericMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            [Message(typeof(AnyTenantUpdated))]
            [Message(typeof(AnyTenantRemoved))]
            void PublishTenantEvent<TEvent>(Guid _, TEvent __)
                where TEvent : IEvent;
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantSingleMessagePublisher", Summary = "Publish single domain event about tenants.")]
        public class TenantSingleMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            public void PublishTenantCreated(Guid id, AnyTenantCreated created)
            {
            }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.tenants_history", Description = "Tenant events.")]
        [PublishOperation(OperationId = "TenantSingleMessagePublisher", Summary = "Publish single domain event about tenants.")]
        public interface ITenantSingleMessagePublisher
        {
            [Message(typeof(AnyTenantCreated))]
            public void PublishTenantCreated(Guid _, AnyTenantCreated __);
        }

        [AsyncApi]
        [Channel("asw.tenant_service.{tenant_id}.{tenant_status}", Description = "A tenant events.")]
        [ChannelParameter("tenant_id", typeof(long), Description = "The tenant identifier.")]
        [ChannelParameter("tenant_status", typeof(string), Description = "The tenant status.")]
        [SubscribeOperation(OperationId = "OneTenantMessageConsumer", Summary = "Subscribe to domains events about a tenant.")]
        public class OneTenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            public void SubscribeTenantCreatedEvent(Guid id, TenantCreated created) { }

            [Message(typeof(TenantUpdated))]
            public void SubscribeTenantUpdatedEvent(Guid id, TenantUpdated updated) { }

            [Message(typeof(TenantRemoved))]
            public void SubscribeTenantRemovedEvent(Guid id, TenantRemoved removed) { }
        }

        [AsyncApi]
        [Channel("asw.tenant_service.{tenant_id}.{tenant_status}", Description = "A tenant events.")]
        [ChannelParameter("tenant_id", typeof(long), Description = "The tenant identifier.")]
        [ChannelParameter("tenant_status", typeof(string), Description = "The tenant status.")]
        [SubscribeOperation(OperationId = "OneTenantMessageConsumer", Summary = "Subscribe to domains events about a tenant.")]
        public interface IOneTenantMessageConsumer
        {
            [Message(typeof(TenantCreated))]
            void SubscribeTenantCreatedEvent(Guid _, TenantCreated __);

            [Message(typeof(TenantUpdated))]
            void SubscribeTenantUpdatedEvent(Guid _, TenantUpdated __);

            [Message(typeof(TenantRemoved))]
            void SubscribeTenantRemovedEvent(Guid _, TenantRemoved __);
        }
    }

    public class TenantCreated { }

    public class TenantUpdated { }

    public class TenantRemoved { }

    public class MyMessage { }

    public class MyMessageHeader
    {
        [Required]
        public string StringHeader { get; set; }
        public int? NullableIntHeader { get; set; }
    }
}
