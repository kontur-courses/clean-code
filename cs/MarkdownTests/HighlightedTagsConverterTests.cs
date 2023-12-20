using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests;

public class HighlightedTagsConverterTests
{
    private HighlightedData highlightedData;
    private HighlightedTagsConverter highlightedTagsConverter;

    [SetUp]
    public void Setup()
    {
        var tags = new HashSet<ITag>
        {
            new EmTag(),
            new StrongTag(),
            new HeaderTag()
        };

        highlightedData = new TagsHighlighter(tags).HighlightMdTags("# Заголовок __с _разными_ символами__");
        highlightedTagsConverter = new HighlightedTagsConverter(tags);
    }

    [Test]
    public void ToHTMLCode()
    {
        highlightedTagsConverter.ToHTMLCode(highlightedData)
            .Should().BeEquivalentTo("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }
}