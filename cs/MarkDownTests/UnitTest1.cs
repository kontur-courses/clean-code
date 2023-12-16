using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace MarkDownTests;

public class MarkDownTests
{
    private MarkDownEnvironment markDownEnv;
    
    [SetUp]
    public void Setup()
    {
        markDownEnv = new MarkDownEnvironment();
    }

    [TestCase("# a", "<h1>a</h1>", TestName = ("HandleHeaderLine"))]
    public void GenerateHtml_Should(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
}