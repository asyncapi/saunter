using AsyncApi.Net.Generator;

using AsyncApiLibrary.Schema.v2;

using Newtonsoft.Json.Schema;

AsyncApiDocument asyncApiDocument = new(new("tester")
{
    Version = "1.0.0",
    License = new("helpme"),
});

// gen to servers, channels, tags to automate link on component and main docs

Console.WriteLine(AsyncApiStore.Messages.MessageSSSSS);
Console.WriteLine(AsyncApiStore.Channels.MyTester2);
Console.WriteLine(AsyncApiStore.Operations.MyTesterOperation);

public record DtoSample(string Name);

public class Tester
{
    [AsyncApiMessage]
    public readonly static Message MessageSSSSS = new()
    {
        ContentType = "text/plain",
        Summary = "its my content",
        Payload = new JSchema(),
    };

    [AsyncApiOperation]
    public readonly static Operation MyTesterOperation = new("ss", AsyncApiStore.Messages.MessageSSSSS)
    {
        Summary = "test operation",
    };

    [AsyncApiChannel]
    public readonly static ChannelItem MyTester2 = new("MyTester2.${Cluster}")
    {
        Description = "description",
        Publish = AsyncApiStore.Operations.MyTesterOperation,
        Parameters = new() { { "Cluster", new() { } } }
    };
}
