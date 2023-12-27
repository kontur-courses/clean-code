using FluentAssertions;
using Markdown;
using Markdown.Tags;
using Markdown.Tokens;

namespace MdTest20;

public class ParserTests
{
    private readonly Dictionary<string, TagType> tagDictionary = new()
    {
        { "_", TagType.Italic },
        { "__", TagType.Bold },
        { "# ", TagType.Heading },
        { "* ", TagType.Bulleted },
        { "## ", TagType.Heading },
        { "### ", TagType.Heading }
    };
    private static IEnumerable<TestCaseData> ConstructorParserExpectedTokenList => new[]
    {
        new TestCaseData("text",
                new List<Token> { new("text", null, TokenType.Text), new("\n", null, TokenType.LineBreaker) })
            .SetName("ShouldReturnText_WhenThereOnlyText"),
        new TestCaseData("_tex_t",
                new List<Token>
                {
                    new("_", Tag.CreateTag(TagType.Italic, "_", null, "t"), TokenType.Tag),
                    new("tex", null, TokenType.Text),
                    new("_", Tag.CreateTag(TagType.Italic, "_", new Token("tex", null, TokenType.Text), null),
                        TokenType.Tag),
                    new("t", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("ShouldReturnItalicToken_WhenThereIsItalicToken"),
        new TestCaseData(@"text\_",
                new List<Token>
                {
                    new("text", null, TokenType.Text),
                    new(@"\", null, TokenType.Escape),
                    new("_", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("ShouldReturnText_WhenThereIsEscapeTokenBeforeTagToken"),
        new TestCaseData(@"te\\xt",
                new List<Token>
                {
                    new("te", null, TokenType.Text),
                    new(@"\", null, TokenType.Escape),
                    new(@"\", null, TokenType.Text),
                    new("xt", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("ShouldReturnDoubleEscapeToken_WhenThereIsDoubleEscapeTokenInText"),
        new TestCaseData(@"te\xt",
                new List<Token>
                {
                    new("te", null, TokenType.Text),
                    new(@"\", null, TokenType.Text),
                    new("xt", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("ShouldReturnEscapeToken_WhenThereIsEscapeToken"),
        new TestCaseData("__tex_t",
                new List<Token>
                {
                    new("__", Tag.CreateTag(TagType.Bold, "__", null, "t"), TokenType.Tag),
                    new("tex", null, TokenType.Text),
                    new("_", Tag.CreateTag(TagType.Italic, "_", new Token("tex", null, TokenType.Text), null),
                        TokenType.Tag),
                    new("t", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("ShouldReturnEscapeToken_WhenThereIsTagMixInText")
    };

    [TestCaseSource(nameof(ConstructorParserExpectedTokenList))]
    public void ParseTests(string text, List<Token> expectedTokens)
    {
        var parser = new Parser(tagDictionary);
        parser.Parse(text).Should().BeEquivalentTo(expectedTokens);
    }
}