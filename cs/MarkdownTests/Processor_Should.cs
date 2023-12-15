using FluentAssertions;
using Markdown.Processor;
using Markdown.Syntax;
using Markdown.Token;

namespace MarkdownTests;

public class Tests
{
    private ISyntax syntax;

    [OneTimeSetUp]
    public void Setup()
    {
        syntax = new Syntax();
    }

    [Test]
    public void Parse_Headers()
    {
        var expected = new List<MarkdownToken>()
            { new MarkdownToken(0, TagType.Header, 1), new MarkdownToken(9, TagType.Header, 1) };
        var text = "#Header1\n#header2";

        var sut = new Processor(text, syntax);

        sut.ParseTags().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Parse_Underline()
    {
        var expected = new List<MarkdownToken>()
        {
            new MarkdownToken(0, TagType.Italic, 1), new MarkdownToken(5, TagType.Italic, 1),
            new MarkdownToken(8, TagType.Italic, 1), new MarkdownToken(11, TagType.Italic, 1)
        };
        var text = "_text_wi_th_underlines";

        var sut = new Processor(text, syntax);

        sut.ParseTags().Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Parse_DoubleUnderline()
    {
        var expected = new List<MarkdownToken>()
        {
            new MarkdownToken(0, TagType.Bold, 2), new MarkdownToken(6, TagType.Bold, 2),
            new MarkdownToken(12, TagType.Bold, 2), new MarkdownToken(20, TagType.Bold, 2)
        };
        var text = "__text__with__double__underline";
        
        var sut = new Processor(text, syntax);

        sut.ParseTags().Should().BeEquivalentTo(expected);
    }
}