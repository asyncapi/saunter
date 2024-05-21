using Saunter.Attributes;

namespace Saunter.Tests.MarkerTypeTests
{
    [AsyncApi]
    [Channel("asw.sample_service.anothersample", Description = "Another sample events.")]
    [PublishOperation(OperationId = "AnotherSampleMessagePublisher", Summary = "Publish another sample.")]
    public class AnotherSamplePublisher
    {
        [Message(typeof(AnotherSampleMesssage))]
        public void PublishTenantCreated(AnotherSampleMesssage _) { }
    }

    [AsyncApi]
    [Channel("asw.sample_service.sample", Description = "Sample events.")]
    [SubscribeOperation(OperationId = "SampleMessageConsumer", Summary = "Consume sample messages.")]
    public class SampleConsumer
    {
        [Message(typeof(SampleMessage))]
        public void SubscribeSampleMessage(SampleMessage _) { }
    }

    public class SampleMessage { }
    public class AnotherSampleMesssage { }
}
