using AsyncApi.Net.Generator;

Console.WriteLine("helpme");
Console.WriteLine(AsyncApiDocument.Operations.MyTester2);

public class Tester
{
    [AsyncApiOperation]
    public static readonly AsyncApiOperationDocument MyTester2 = new()
    {
        TestValue = "sss"
    };
}
