using AsyncApi.Net.Generator;

using AsyncApiLibrary.Schema.v2;

AsyncApiDocument asyncApiDocument = new(new("tester")
{
Version = "1.0.0",
License = new("helpme"),
})
{
    
};

// gen to servers, channels, tags to automate link on component and main docs


Console.WriteLine(asyncApiDocument.Channels.MyTester2.Description);

public class Tester
{
    [AsyncApiChannel]
    public static readonly ChannelItem MyTester2 = new()
    {
        Description = "description",
    };
}
