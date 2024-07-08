using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Saunter.Attributes;
using Saunter.Generation;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.DocumentGeneratorTests
{
    public class ClassAttributesTests
    {
        [Theory]
        [InlineData(typeof(TenantMessageConsumer))]
        [InlineData(typeof(ITenantMessageConsumer))]
        public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel(Type type)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[] { type.GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

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

            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
        }


        [Theory]
        [InlineData(typeof(TenantGenericMessagePublisher))]
        [InlineData(typeof(ITenantGenericMessagePublisher))]
        public void GenerateDocument_GeneratesDocumentWithMultipleMessagesPerChannelInTheSameMethod(Type type)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[] { type.GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

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

            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantCreated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantUpdated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "anyTenantRemoved");
        }


        [Theory]
        [InlineData(typeof(TenantSingleMessagePublisher))]
        [InlineData(typeof(ITenantSingleMessagePublisher))]
        public void GenerateDocument_GeneratesDocumentWithSingleMessage(Type type)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[] { type.GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

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

            var message = publish.Message.ShouldBeOfType<MessageReference>();
            message.Id.ShouldBe("anyTenantCreated");
        }


        [Theory]
        [InlineData(typeof(TenantMessageConsumer), typeof(TenantMessagePublisher))]
        [InlineData(typeof(ITenantMessageConsumer), typeof(ITenantMessagePublisher))]
        public void GetDocument_WhenMultipleClassesUseSameChannelKey_GeneratesDocumentWithMultipleMessagesPerChannel(Type type1, Type type2)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[]
            {
                type1.GetTypeInfo(),
                type2.GetTypeInfo()
            }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

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

            subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            subscribeMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");

            var publishMessages = subscribe.Message.ShouldBeOfType<Messages>();
            publishMessages.OneOf.Count.ShouldBe(3);

            publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            publishMessages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
        }


        [Theory]
        [InlineData(typeof(OneTenantMessageConsumer))]
        [InlineData(typeof(IOneTenantMessageConsumer))]
        public void GenerateDocument_GeneratesDocumentWithChannelParameters(Type type)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[] { type.GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);

            var channel = document.Channels.First();
            channel.Key.ShouldBe("asw.tenant_service.{tenant_id}.{tenant_status}");
            channel.Value.Description.ShouldBe("A tenant events.");
            channel.Value.Parameters.Count.ShouldBe(2);
            channel.Value.Parameters.Values.OfType<ParameterReference>().ShouldContain(p => p.Id == "tenant_id" && p.Value.Schema != null && p.Value.Description == "The tenant identifier.");
            channel.Value.Parameters.Values.OfType<ParameterReference>().ShouldContain(p => p.Id == "tenant_status" && p.Value.Schema != null && p.Value.Description == "The tenant status.");

            var subscribe = channel.Value.Subscribe;
            subscribe.ShouldNotBeNull();
            subscribe.OperationId.ShouldBe("OneTenantMessageConsumer");
            subscribe.Summary.ShouldBe("Subscribe to domains events about a tenant.");

            var messages = subscribe.Message.ShouldBeOfType<Messages>();
            messages.OneOf.Count.ShouldBe(3);

            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantCreated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantUpdated");
            messages.OneOf.OfType<MessageReference>().ShouldContain(m => m.Id == "tenantRemoved");
        }


        [Theory]
        [InlineData(typeof(MyMessagePublisher))]
        [InlineData(typeof(IMyMessagePublisher))]
        public void GenerateDocument_GeneratesDocumentWithMessageHeader(Type type)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[] { type.GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

            // Assert
            document.ShouldNotBeNull();

            document.Components.Schemas.Values.ShouldContain(t => t.Id == "myMessageHeader");
            var message = document.Components.Messages.Values.First();
            message.Headers.Reference.Id.ShouldBe("myMessageHeader");
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
        [Channel("channel.my.message")]
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
