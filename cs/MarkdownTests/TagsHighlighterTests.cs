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

        tagsHighlighter.FindTagIndexes(new EmTag(), ref actual);

        var expected = new List<PairTagInfo>
        {
            new(0, 4),
            new(14, 29),
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FindOnlyTagIndexesStrong()
    {
        tagsHighlighter.MarkdownText = "___a__ __b__as __a__ c__";

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindTagIndexes(new StrongTag(), ref actual);

        var expected = new List<PairTagInfo>
        {
            new(0, 4),
            new(7, 18)
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void FindCorrectHeaderPosition()
    {
        tagsHighlighter.MarkdownText = "#asfsf asf gewg e\t #asxf # \n#.";

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindTagIndexes(new HeaderTag(), ref actual);

        var expected = new List<PairTagInfo> { new(0, 28) };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IgnoreTagWithShielding()
    {
        tagsHighlighter.MarkdownText = "\\_a_";

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindTagIndexes(new EmTag(), ref actual);

        actual.Should().BeEmpty();
    }

    [Test]
    public void SingleTagsIndexesDictCheck()
    {
        tagsHighlighter.MarkdownText = "#";
        var actual = tagsHighlighter.TagsIndexes();

        var expected = new Dictionary<Type, List<int>>
        {
            { typeof(HeaderTag), new List<int> { 0 } }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void PairTagsIndexesDictCheck()
    {
        tagsHighlighter.MarkdownText = "_a_ __b__";
        var actual = tagsHighlighter.TagsIndexes();

        var expected = new Dictionary<Type, List<PairTagInfo>>
        {
            { typeof(EmTag), new List<PairTagInfo> { new(0, 2) } },
            { typeof(StrongTag), new List<PairTagInfo> { new(4, 7) } }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IgnoreIntersectionStrongAndEmTags()
    {
        tagsHighlighter.MarkdownText = "__пересечения _двойных__ и одинарных_";

        var actual = tagsHighlighter.TagsIndexes();

        tagsHighlighter.RemoveIntersectStrongAndEmTags(ref actual);

        var expected = new Dictionary<Type, List<PairTagInfo>>
        {
            { typeof(EmTag), new List<PairTagInfo>() },
            { typeof(StrongTag), new List<PairTagInfo>() }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IgnoreStrongInsideEmTag()
    {
        tagsHighlighter.MarkdownText = "внутри _одинарного __двойное__ не_ работает";

        var actual = tagsHighlighter.TagsIndexes();

        tagsHighlighter.RemoveStrongInsideEmTags(ref actual);

        var expected = new Dictionary<Type, List<PairTagInfo>>
        {
            { typeof(EmTag), new List<PairTagInfo> { new(7, 33) } },
            { typeof(StrongTag), new List<PairTagInfo>() }
        };

        actual.Should().BeEquivalentTo(expected);
    }
}