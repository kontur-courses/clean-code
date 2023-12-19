using FluentAssertions;
using Markdown;
using Markdown.TagClasses;

namespace MarkdownTests;

[TestFixture]
public class MarkdownCreator_Should
{
    private static Tag[] tags = { new BoldTag(), new EscapeTag(), new HeaderTag(), new ItalicTag() };
    private Parser parser;
    private Renderer renderer;
    private MarkdownCreator markdownCreator;

    [SetUp]
    public void CreateNewSplitterInstance()
    {
        parser = new Parser(tags);
        renderer = new Renderer();

        markdownCreator = new MarkdownCreator(parser, renderer);
    }

    [TestCase("_Lorem ipsum dolor_", ExpectedResult = "<em>Lorem ipsum dolor</em>")]
    [TestCase("__Lorem ipsum dolor__", ExpectedResult = "<strong>Lorem ipsum dolor</strong>")]
    [TestCase("# Lorem ipsum dolor\n", ExpectedResult = "<h1>Lorem ipsum dolor</h1>")]
    public string MarkdownTextMarkdownsSimpleTags(string inputString)
    {
        var outputString = markdownCreator.MarkdownText(inputString);
        return outputString;
    }

    [TestCase(@"\_Lorem_ ipsum _dolor_ sit amet", TestName = "EscapedTag", ExpectedResult = @"_Lorem_ ipsum <em>dolor</em> sit amet")]
    [TestCase(@"_Lorem_ ipsum _do\lor_ sit amet", TestName = "EscapedNothing", ExpectedResult = @"<em>Lorem</em> ipsum <em>do\lor</em> sit amet")]
    [TestCase(@"\\_Lorem ipsum_ _dolor_ sit amet", TestName = "EscapedEscape", ExpectedResult = @"\<em>Lorem ipsum</em> <em>dolor</em> sit amet")]
    public string MarkdownTextMarkdownsEscapeInText(string inputString)
    {
        var outputString = markdownCreator.MarkdownText(inputString);
        return outputString;
    }


    [Test]
    [Timeout(1000)]
    public void MarkdownTextIsLinear()
    {
        var text = File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "LinearTestData.txt"));

        var outputString = markdownCreator.MarkdownText(text);
        var expectedResult = File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "LinearTestExpectedOutput.txt"));
        outputString.Should().Be(expectedResult);
    }
}