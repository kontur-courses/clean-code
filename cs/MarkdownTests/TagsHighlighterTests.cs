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

    [Test]
    public void IgnoreTagWithShielding()
    {
        tagsHighlighter.MarkdownText = "\\_a_";

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindPairTagIndexes(new EmTag(), ref actual);

        actual.Should().BeEmpty();
    }

    [Test]
    public void SingleTagsIndexesDictCheck()
    {
        var tags = new List<ITag> { new HeaderTag() };
        var actualTagsHighlighter = new TagsHighlighter(tags)
        {
            MarkdownText = "#"
        };

        var actual = actualTagsHighlighter.SingleTagsIndexes();

        var expected = new Dictionary<ISingleTag, List<int>>
        {
            { (ISingleTag)tags[0], new List<int> { 0 } }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void PairTagsIndexesDictCheck()
    {
        var tags = new List<ITag> { new EmTag(), new StrongTag() };
        var actualTagsHighlighter = new TagsHighlighter(tags)
        {
            MarkdownText = "_a_ __b__"
        };

        var actual = actualTagsHighlighter.PairTagsIndexes();

        var expected = new Dictionary<IPairTag, List<PairTagInfo>>
        {
            { (IPairTag)tags[0], new List<PairTagInfo> { new((0, 2)) } },
            { (IPairTag)tags[1], new List<PairTagInfo> { new((4, 7)) } }
        };

        actual.Should().BeEquivalentTo(expected);
    }
}