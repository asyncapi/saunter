using System;
using System.Linq;
using System.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.DocumentGeneratorTests
{
    public class InterfaceAttributeTests
    {
        [Theory]
        [InlineData(typeof(IServiceEvents))]
        [InlineData(typeof(ServiceEventsFromInterface))]
        [InlineData(typeof(ServiceEventsFromAnnotatedInterface))] // Check that annotations are not inherited from the interface
        public void NonAnnotatedTypesTest(Type type)
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            var document = documentGenerator.GenerateDocument(new[] { type.GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData(typeof(IAnnotatedServiceEvents), "interface")]
        [InlineData(typeof(AnnotatedServiceEventsFromInterface), "class")]
        [InlineData(typeof(AnnotatedServiceEventsFromAnnotatedInterface), "class")] // Check that the actual type's annotation takes precedence of the inherited interface
        public void AnnotatedTypesTest(Type type, string source)
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
            channel.Key.ShouldBe($"{source}.event");
            channel.Value.Description.ShouldBeNull();

            var publish = channel.Value.Publish;
            publish.ShouldNotBeNull();
            publish.OperationId.ShouldBe("PublishEvent");
            publish.Description.ShouldBe($"({source}) Subscribe to domains events about a tenant.");

            var messageRef = publish.Message.ShouldBeOfType<MessageReference>();
            messageRef.Id.ShouldBe("tenantEvent");
        }

        [AsyncApi]
        private interface IAnnotatedServiceEvents
        {
            [Channel("interface.event")]
            [PublishOperation(typeof(TenantEvent), Description = "(interface) Subscribe to domains events about a tenant.")]
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
        private class AnnotatedServiceEventsFromInterface : IAnnotatedServiceEvents
        {
            [Channel("class.event")]
            [PublishOperation(typeof(TenantEvent), Description = "(class) Subscribe to domains events about a tenant.")]
            public void PublishEvent(TenantEvent evt) { }
        }

        [AsyncApi]
        private class AnnotatedServiceEventsFromAnnotatedInterface : IAnnotatedServiceEvents
        {
            [Channel("class.event")]
            [PublishOperation(typeof(TenantEvent), Description = "(class) Subscribe to domains events about a tenant.")]
            public void PublishEvent(TenantEvent evt) { }
        }

        private class TenantEvent { }
    }
}
