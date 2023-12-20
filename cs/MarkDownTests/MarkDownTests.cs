namespace MarkDownTests;

public class MarkDownTests
{
    private MarkDownEnvironment markDownEnv;
    
    [SetUp]
    public void Setup()
    {
        markDownEnv = new MarkDownEnvironment();
    }

    [TestCase("\n# a", "\n<h1>a</h1>", TestName = "IsCreatedWhenNoNewLineAtTheEnd")]
    [TestCase("\n# a \n", "\n<h1>a </h1>", TestName = "IsCreatedWhenNewLineAtTheEnd")]
    [TestCase("\n# a \n", "\n<h1>a </h1>", TestName = "IsCreatedWhenNewLineAtTheEnd")]
    [TestCase("# a \n", "<h1>a </h1>", TestName = "IsCreatedWhenNoNewLineAtTheStart")]
    [TestCase(" # a \n", " # a \n", TestName = "IsCreatedWhenIncorrectStart")]
    [TestCase("# __a__\n", "<h1><strong>a</strong></h1>", TestName = "SupportsAnotherTagsInsideWhenNewLineAtTheEnd")]
    [TestCase("# __a__", "<h1><strong>a</strong></h1>", TestName = "SupportsAnotherTagsInsideWhenNoNewLineAtTheEnd")]
    public void GenerateHtml_Header(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
    
    [TestCase("__a__", "<strong>a</strong>", TestName = "CreatesTag")]
    [TestCase("a__a__ ", "a<strong>a</strong> ", TestName = "CreatesTagInWord")]
    [TestCase("a __bcd__ e", "a <strong>bcd</strong> e", TestName = "CreatesTagInPhrase")]
    [TestCase("__a __", "__a __", TestName = "DoesNotCloseIncorrectTag")]
    [TestCase("__ a__", "__ a__", TestName = "DoesNotOpenIncorrectTag")]
    [TestCase("__a1__", "__a1__", TestName = "CannotOpenWithDigits")]
    public void GenerateHtml_Strong(string markdown, string expected)
    {
        MarkDown.MarkDown.GenerateHtml(markdown, markDownEnv)
            .Should()
            .Be(expected);
    }
}