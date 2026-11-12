using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class InlineParserTests
{
    private readonly InlineParser parser = new([new StrongTag(), new EmphasisTag()]);

    [Test]
    public void Parse_StrongAroundEmphasis_NestsTags()
    {
        parser.Parse("__с _разными_ символами__")
            .Should().Be("<strong>с <em>разными</em> символами</strong>");
    }

    [Test]
    public void Parse_DigitsAroundUnderscore_DoNotOpenTag()
    {
        parser.Parse("1_2_3").Should().Be("1_2_3");
    }

    [Test]
    public void Parse_UnderscoreInsideOneWord_OpensEmphasis()
    {
        parser.Parse("a_12_b").Should().Be("a<em>12</em>b");
    }

    [Test]
    public void Parse_EscapedBackslash_LeavesFollowingUnderscoreActive()
    {
        parser.Parse(@"\\_текст_")
            .Should().Be(@"\<em>текст</em>");
    }
}