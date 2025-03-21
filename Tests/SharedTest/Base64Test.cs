using SharedLib;

namespace SharedTest;

public class Base64Test
{
    [Test]
    public void TestParsing()
    {
        string normal = "Readable String";
        string b64 = normal.ToB64();
        Assert.That(normal != b64);
        string parsedBack = b64.FromB64();
        Assert.That(normal == parsedBack);
    }
}
