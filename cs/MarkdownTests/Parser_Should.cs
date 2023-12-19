using FluentAssertions;
using Markdown;
using Markdown.TagClasses;

namespace MarkdownTests;

public class Parser_Should
{
    private static Tag[] tags = {new BoldTag(), new EscapeTag(), new HeaderTag(), new ItalicTag(), new NewLineTag(), new ListElement()};
    private Parser parser;

    [SetUp]
    public void CreateNewSplitterInstance()
    {
        parser = new Parser(tags);
    }

    [TestCase("_Lorem ipsum dolor_")]
    [TestCase("__Lorem ipsum dolor__")]
    [TestCase("# Lorem ipsum dolor\n")]
    public void GetTagsToRenderParsesOneTagPair(string inputString)
    {
        var tagsToRender = parser.GetTagsToRender(inputString);
        tagsToRender.Should().HaveCount(2);
        tagsToRender.First().StartIndex.Should().Be(0);
    }

    [Test]
    public void GetTagsToRenderSkipsEscapedTags()
    {
        var tagsToRender = parser.GetTagsToRender("\\_Lorem_ ipsum _dolor_ sit amet").ToArray();
        tagsToRender.Should().HaveCount(3);
    }

    [Test]
    public void GetTagsToRenderEscapesOnlyTags()
    {
        var tagsToRender = parser.GetTagsToRender("_Lorem_ ipsum _do\\lor_ sit amet");
        tagsToRender.Should().HaveCount(4);
    }

    [Test]
    public void GetTagsToRenderEscapesEscapedTags()
    {
        var tagsToRender = parser.GetTagsToRender("\\\\_Lorem ipsum_ _dolor_ sit amet ");
        tagsToRender.Should().HaveCount(5);
    }

    [Test]
    public void GetTagsToRenderParsesInsideWords()
    {
        var tagsToRender = parser.GetTagsToRender("_Lor_em ip_s_um dol_or._");
        tagsToRender.Should().HaveCount(6);
    }

    [TestCase("Lorem_1_2 123_456_789", TestName = "TagsInsideNumbers")]
    [TestCase("Lor_em ips_um", TestName = "TagPairInSeparateWords")]
    [TestCase("_Lorem __ipsum_ dolor__", TestName = "IntersectingTags")]
    [TestCase("Lorem __ipsum_", TestName = "UnpairedTags")]
    [TestCase("Lorem__ ipsum__", TestName = "WrongOpenedTags")]
    [TestCase("Lorem __ipsum __sit", TestName = "WrongClosedTags")]
    [TestCase("____ __", TestName = "EmptyTagPair")]
    public void GetTagsToRenderSkips(string testingString)
    {
        var tagsToRender = parser.GetTagsToRender(testingString);
        tagsToRender.Should().HaveCount(0);
    }

    [Test]
    public void GetTagsToRenderParsesItalicInBold()
    {
        var tagsToRender = parser.GetTagsToRender("Lorem __ipsum _dolor_ sit__");
        tagsToRender.Should().HaveCount(4);
    }

    [Test]
    public void GetTagsToRenderSkipsBoldInItalic()
    {
        var tagsToRender = parser.GetTagsToRender("Lorem _ipsum __dolor__ sit_");
        tagsToRender.Should().HaveCount(2);
    }

    [Test]
    public void GetTagsToRenderParsesMultiplePairsInHeader()
    {
        var tagPairs = parser.GetTagsToRender("# Lorem __ipsum _sit_ amet__\n");
        tagPairs.Should().HaveCount(6);
    }
}