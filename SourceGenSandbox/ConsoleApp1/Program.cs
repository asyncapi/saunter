using AsyncApi.Net.Generator;

Console.WriteLine("helpme");
Console.WriteLine(AsyncApiDocument.Operations.MyTester2);

public class Tester
{
    [AsyncApiOperation]
    private static readonly AsyncApiOperationDocument _myTester2 = new()
    {
        TestValue = "sss"
    };
}
