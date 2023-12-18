using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests;

public class TagsHighlighterTests
{
    private TagsHighlighter tagsHighlighter;

    [SetUp]
    public void Setup()
    {
        var tags = new List<ITag>
        {
            new EmTag(),
            new StrongTag(),
            new HeaderTag()
        };

        tagsHighlighter = new TagsHighlighter(tags);
    }

    [Test]
    public void FindOnlyTagIndexesEm()
    {
        tagsHighlighter.MarkdownText = "_agd_ __asd__ _a__asd_s fa2_4_";

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindPairTagIndexes(new EmTag(), ref actual);

        var expected = new List<PairTagInfo>
        {
            new((0, 4)),
            new((14, 29)),
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FindOnlyTagIndexesStrong()
    {
        tagsHighlighter.MarkdownText = "___a__ __b__as __a__ c__";

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindPairTagIndexes(new StrongTag(), ref actual);

        var expected = new List<PairTagInfo>
        {
            new((0, 4)),
            new((7, 18))
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FindCorrectHeaderPosition()
    {
        tagsHighlighter.MarkdownText = "#asfsf asf gewg e\t #asxf # \n#.";

        var actual = new List<int>();

        tagsHighlighter.FindSingleTagIndexes(new HeaderTag(), ref actual);

        var expected = new List<int> { 0, 28 };

        actual.Should().BeEquivalentTo(expected);
    }
}