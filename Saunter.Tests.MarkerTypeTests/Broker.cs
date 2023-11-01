using Saunter.Attributes;

namespace Saunter.Tests.MarkerTypeTests;

[PublishOperation("asw.sample_service.anothersample", OperationId = "AnotherSampleMessagePublisher", Summary = "Publish another sample.", ChannelDescription = "Another sample events.")]
public class AnotherSamplePublisher
{
    [Message(typeof(AnotherSampleMesssage))]
    public void PublishTenantCreated(AnotherSampleMesssage @event) { }
}

[PublishOperation("asw.sample_service.sample", OperationId = "AnotherSampleMessagePublisher", Summary = "Publish another sample.", ChannelDescription = "Sample events.")]
[SubscribeOperation("asw.sample_service.sample", OperationId = "SampleMessageConsumer", Summary = "Consume sample messages.")]
public class SampleConsumer
{
    [Message(typeof(SampleMessage))]
    public void SubscribeSampleMessage(SampleMessage evnt) { }
}

public class SampleMessage { }
public class AnotherSampleMesssage { }