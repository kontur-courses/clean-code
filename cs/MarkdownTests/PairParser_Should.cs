using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class PairParser_Should
{
    private static Tag[] tags = Tags.GetAllTags().ToArray();
    private Parser parser;

    [SetUp]
    public void CreateNewSplitterInstance()
    {
        parser = new Parser(tags);
    }

    [TestCase("_Lorem ipsum dolor_")]
    [TestCase("__Lorem ipsum dolor__")]
    [TestCase("# Lorem ipsum dolor\n")]
    public void ParseTagPairsParsesOneTagPair(string inputString)
    {
        var tagsToRender = parser.GetTagsToRender(inputString);
        tagsToRender.Should().HaveCount(2); 
        tagsToRender.First().StartIndex.Should().Be(0);
    }

    [Test]
    public void ParseTagPairsSkipsEscapedTags()
    {
        var tagsToRender = parser.GetTagsToRender("\\_Lorem_ ipsum _dolor_ sit amet").ToArray();
        tagsToRender.Should().HaveCount(3);
    }

    [Test]
    public void ParseTagPairsEscapesOnlyTags()
    {
        var tagsToRender = parser.GetTagsToRender("_Lorem_ ipsum _do\\lor_ sit amet");
        tagsToRender.Should().HaveCount(4);
    }

    [Test]
    public void ParseTagPairsEscapesEscapedTags()
    {
        var tagsToRender = parser.GetTagsToRender("\\\\_Lorem ipsum_ _dolor_ sit amet");
        tagsToRender.Should().HaveCount(5);
    }

    [Test]
    public void ParseTagPairsParsesInsideWords()
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
    public void ParseTagPairsSkips(string testingString)
    {
        var tagsToRender = parser.GetTagsToRender(testingString);
        tagsToRender.Should().HaveCount(0);
    }

    [Test]
    public void ParseTagPairsParsesItalicInBold()
    {
        var tagsToRender = parser.GetTagsToRender("Lorem __ipsum _dolor_ sit__");
        tagsToRender.Should().HaveCount(4);
    }

    [Test]
    public void ParseTagPairsSkipsBoldInItalic()
    {
        var tagsToRender = parser.GetTagsToRender("Lorem _ipsum __dolor__ sit_");
        tagsToRender.Should().HaveCount(2);
    }

    [Test]
    public void ParseTagPairsParsesMultiplePairsInHeader()
    {
        var tagPairs = parser.GetTagsToRender("# Lorem __ipsum _sit_ amet__\n");
        tagPairs.Should().HaveCount(6);
    }
}