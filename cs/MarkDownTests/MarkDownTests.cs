namespace MarkDownTests;

public class MarkDownTests
{
    private MarkDownEnvironment markDownEnv;
    
    [SetUp]
    public void Setup()
    {
        markDownEnv = new MarkDownEnvironment();
    }

    [TestCase("\n# a", "\n<h1>a</h1>", TestName = "WithoutNewLineAtTheEnd")]
    [TestCase("\n# a \n", "\n<h1>a </h1>", TestName = "WithNewLineAtTheEnd")]
    [TestCase("# a \n", "<h1>a </h1>", TestName = "WithoutNewLineAtTheStart")]
    [TestCase(" # a \n", " # a \n", TestName = "WithIncorrectStart")]
    public void GenerateHtml_WithHeader(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    
}