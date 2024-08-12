using LEGO.AsyncAPI.Models;
using Shouldly;

namespace Saunter.Tests.AttributeProvider.DocumentGenerationTests
{
    internal static class AssertAsyncApiDocumentHelper
    {
        public static AsyncApiChannel AssertAndGetChannel(this AsyncApiDocument document, string key, string description)
        {
            document.Channels.Count.ShouldBe(1);
            document.Channels.ShouldContainKey(key);

            var channel = document.Channels[key];
            channel.ShouldNotBeNull();
            channel.Description.ShouldBe(description);

            return channel;
        }

        public static void AssertByMessage(this AsyncApiDocument document, AsyncApiOperation operation, params string[] messageIds)
        {
            operation.Message.Count.ShouldBe(messageIds.Length);
            operation.Message.ShouldAllBe(c => c.Reference.Type == ReferenceType.Message);

            foreach (var messageId in messageIds)
            {
                operation.Message.ShouldContain(m => m.Reference.Id == messageId);
                document.Components.Messages.ShouldContainKey(messageId);

                var message = document.Components.Messages[messageId];
                document.Components.Schemas.ContainsKey(message.Payload.Reference.Id);
            }
        }
    }
}
