using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests;

public class TagsHighlighterTests
{
    [Test]
    public void FindOnlyTagIndexesEm()
    {
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new EmTag(),
                new StrongTag()
            },
            "_agd_ __asd__ _a__asd_s fa2_4_");

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
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new EmTag(),
                new StrongTag()
            },
            "___a__ __b__as __a__ c__");

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
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new HeaderTag()
            },
            "asfsf asf gewg e\t #asxf # \n# a");

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindTagIndexes(new HeaderTag(), ref actual);

        var expected = new List<PairTagInfo> { new(27, 30) };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IgnoreTagWithShielding()
    {
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new EmTag()
            },
            "\\_a_");

        var actual = new List<PairTagInfo>();

        tagsHighlighter.FindTagIndexes(new EmTag(), ref actual);

        actual.Should().BeEmpty();
    }

    [Test]
    public void HeaderIndexesDictCheck()
    {
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new HeaderTag()
            },
            "# ");

        var actual = tagsHighlighter.TagsIndexes();

        var expected = new Dictionary<Type, List<PairTagInfo>>
        {
            { typeof(HeaderTag), new List<PairTagInfo> { new(0, 2) } }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TagsIndexesDictCheck()
    {
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new EmTag(),
                new StrongTag()
            },
            "_a_ __b__");

        var actual = tagsHighlighter.TagsIndexes();

        var expected = new Dictionary<Type, List<PairTagInfo>>
        {
            { typeof(EmTag), new List<PairTagInfo> { new(0, 2) } },
            { typeof(StrongTag), new List<PairTagInfo> { new(4, 7) } },
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void IgnoreIntersectionStrongAndEmTags()
    {
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new EmTag(),
                new StrongTag()
            },
            "__пересечения _двойных__ и одинарных_");

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
        var tagsHighlighter = new TagsHighlighter(
            new HashSet<ITag>
            {
                new EmTag(),
                new StrongTag()
            },
            "внутри _одинарного __двойное__ не_ работает");

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