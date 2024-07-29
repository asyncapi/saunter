using System;
using Saunter.AttributeProvider.Attributes;
using Shouldly;
using Xunit;

namespace Saunter.Tests.AttributeProvider.DocumentGenerationTests
{
    public class InterfaceAttributeTests
    {
        [Theory]
        [InlineData(typeof(IServiceEvents))]
        [InlineData(typeof(ServiceEventsFromInterface))]
        [InlineData(typeof(ServiceEventsFromAnnotatedInterface))]
        // Check that annotations are not inherited from the interface
        public void NonAnnotatedTypesTest(Type type)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData(typeof(IAnnotatedServiceEvents), "interface.event.service.anotated.interface")]
        [InlineData(typeof(AnnotatedServiceEventsFromAnnotatedInterface), "class.event.service.anotated.interface")]
        [InlineData(typeof(SecondAnnotatedServiceEventsFromAnnotatedInterface), "class.event.secondservice.anotated.interface")]
        // Check that the actual type's annotation takes precedence of the inherited interface
        public void AnnotatedTypesTest(Type type, string channelName)
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, type);

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();

            var channel = document.AssertAndGetChannel(channelName, null);

            var publish = channel.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("PublishEvent");
            publish.Description.ShouldBe($"({channelName}) Subscribe to domains events about a tenant.");

            document.AssertByMessage(publish, "tenantEvent");
        }

        [AsyncApi]
        private interface IAnnotatedServiceEvents
        {
            [Channel("interface.event.service.anotated.interface")]
            [PublishOperation(typeof(TenantEvent), Description = "(interface.event.service.anotated.interface) Subscribe to domains events about a tenant.")]
            void PublishEvent(TenantEvent evt);
        }

        private interface IServiceEvents
        {
            void PublishEvent(TenantEvent evt);
        }

        private class ServiceEventsFromInterface : IServiceEvents
        {
            public void PublishEvent(TenantEvent evt) { }
        }

        private class ServiceEventsFromAnnotatedInterface : IAnnotatedServiceEvents
        {
            public void PublishEvent(TenantEvent evt) { }
        }

        [AsyncApi]
        private class AnnotatedServiceEventsFromAnnotatedInterface : IAnnotatedServiceEvents
        {
            [Channel("class.event.service.anotated.interface")]
            [PublishOperation(typeof(TenantEvent), Description = "(class.event.service.anotated.interface) Subscribe to domains events about a tenant.")]
            public void PublishEvent(TenantEvent evt) { }
        }

        [AsyncApi]
        private class SecondAnnotatedServiceEventsFromAnnotatedInterface : IAnnotatedServiceEvents
        {
            [Channel("class.event.secondservice.anotated.interface")]
            [PublishOperation(typeof(TenantEvent), Description = "(class.event.secondservice.anotated.interface) Subscribe to domains events about a tenant.")]
            public void PublishEvent(TenantEvent evt) { }
        }

        private class TenantEvent { }
    }
}
