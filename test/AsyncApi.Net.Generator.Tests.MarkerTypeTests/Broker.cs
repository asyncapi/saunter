using AsyncApi.Net.Generator.Attributes;

namespace AsyncApi.Net.Generator.Tests.MarkerTypeTests;

[PublishOperation<AnotherSampleMesssage>("asw.sample_service.anothersample", OperationId = "AnotherSampleMessagePublisher", Summary = "Publish another sample.", ChannelDescription = "Another sample events.")]
public class AnotherSamplePublisher
{
    public void PublishTenantCreated(AnotherSampleMesssage @event) { }
}

[PublishOperation<SampleMessage>("asw.sample_service.sample", OperationId = "AnotherSampleMessagePublisher", Summary = "Publish another sample.", ChannelDescription = "Sample events.")]
[SubscribeOperation<SampleMessage>("asw.sample_service.sample", OperationId = "SampleMessageConsumer", Summary = "Consume sample messages.")]
public class SampleConsumer
{
    public void SubscribeSampleMessage(SampleMessage evnt) { }
}

public class SampleMessage { }
public class AnotherSampleMesssage { }