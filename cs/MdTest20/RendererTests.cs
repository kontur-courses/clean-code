using FluentAssertions;
using Markdown;
using Markdown.Tags;
using Markdown.Tokens;

namespace MdTest20;

public class RendererTests
{
    private static IEnumerable<TestCaseData> ConstructorRendererTokenList => new[]
    {
        new TestCaseData(
            new List<Token>
            {
                new("text", null, TokenType.Text),
                new("\n", null, TokenType.LineBreaker)
            },
            new List<Token>
            {
                new("text", null, TokenType.Text),
                new("\n", null, TokenType.LineBreaker)
            }).SetName("whenThereIsOnlyText_ShouldBeReturnText"),
        new TestCaseData(
            new List<Token>
            {
                new("_", Tag.CreateTag(TagType.Italic, "_", null, "t"), TokenType.Tag),
                new("text", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("text", null, TokenType.Text), ""),
                    TokenType.Tag),
                new("\n", null, TokenType.LineBreaker)
            },
            new List<Token>
            {
                new("_", Tag.CreateTag(TagType.Italic, "_", null, "t"), TokenType.Tag)
                {
                    Tag = { TagContent = "<em>", Status = TagStatus.Opening }
                },
                new("text", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("text", null, TokenType.Text), ""), TokenType.Tag)
                {
                    Tag = { TagContent = "</em>", Status = TagStatus.Closing }
                },
                new("\n", null, TokenType.LineBreaker)
            }).SetName("whenThereIsPairedTags_ShouldBeReturnTextWithAllTags"),
        new TestCaseData(new List<Token>
            {
                new("_", Tag.CreateTag(TagType.Italic, "_", null, " "), TokenType.Tag),
                new(" text", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token(" text", null, TokenType.Text), " "),
                    TokenType.Tag)
            },
            new List<Token>
            {
                new("_", Tag.CreateTag(TagType.Italic, "_", null, " "), TokenType.Tag)
                {
                    Tag = { TagContent = "_", Status = TagStatus.Closing }
                },
                new(" text", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token(" text", null, TokenType.Text), ""),
                    TokenType.Tag)
                {
                    Tag = { TagContent = "_", Status = TagStatus.Block }
                }
            }).SetName("whenThereIsClosingTagAtTheBeginning_ShouldBeReturnText"),
        new TestCaseData(new List<Token>
            {
                new("_", Tag.CreateTag(TagType.Italic, "_", null, "t"), TokenType.Tag),
                new("t", null, TokenType.Text),
                new("__", Tag.CreateTag(TagType.Bold, "__", new Token("t", null, TokenType.Text), "e"), TokenType.Tag),
                new("ex", null, TokenType.Text),
                new("__", Tag.CreateTag(TagType.Bold, "__", new Token("ex", null, TokenType.Text), "t"), TokenType.Tag),
                new("t", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("t", null, TokenType.Text), " "), TokenType.Tag)
            },
            new List<Token>
            {
                new("_", Tag.CreateTag(TagType.Italic, "_", null, " "), TokenType.Tag)
                {
                    Tag = { TagContent = "<em>", Status = TagStatus.Opening }
                },
                new("t", null, TokenType.Text),
                new("__", Tag.CreateTag(TagType.Bold, "__", new Token("t", null, TokenType.Text), "e"), TokenType.Tag)
                {
                    Tag = { Status = TagStatus.Opening, TagContent = "<strong>" }
                },
                new("ex", null, TokenType.Text),
                new("__", Tag.CreateTag(TagType.Bold, "__", new Token("ex", null, TokenType.Text), "t"), TokenType.Tag)
                {
                    Tag = { Status = TagStatus.Closing, TagContent = "</strong>" }
                },
                new("t", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("t", null, TokenType.Text), " "), TokenType.Tag)
                {
                    Tag = { TagContent = "</em>", Status = TagStatus.Closing }
                }
            }).SetName("WhenThereIsNestedPairedTags_ShouldBeReturnTextWithAllTags"),
        new TestCaseData(new List<Token>
            {
                new("__", Tag.CreateTag(TagType.Bold, "__", null, "t"), TokenType.Tag),
                new("t", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("t", null, TokenType.Text), "e"), TokenType.Tag),
                new("ex", null, TokenType.Text),
                new("__", Tag.CreateTag(TagType.Bold, "__", new Token("ex", null, TokenType.Text), "t"), TokenType.Tag),
                new("t", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("t", null, TokenType.Text), " "), TokenType.Tag)
            },
            new List<Token>
            {
                new("__", Tag.CreateTag(TagType.Bold, "__", null, " "), TokenType.Tag)
                {
                    Tag = { TagContent = "__", Status = TagStatus.Opening }
                },
                new("t", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("t", null, TokenType.Text), "e"), TokenType.Tag)
                {
                    Tag = { Status = TagStatus.Opening, TagContent = "_" }
                },
                new("ex", null, TokenType.Text),
                new("__", Tag.CreateTag(TagType.Bold, "__", new Token("ex", null, TokenType.Text), "t"), TokenType.Tag)
                {
                    Tag = { Status = TagStatus.Opening, TagContent = "__" }
                },
                new("t", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("t", null, TokenType.Text), " "), TokenType.Tag)
                {
                    Tag = { TagContent = "_", Status = TagStatus.Opening }
                }
            }).SetName("WhenThereIsTheOverlappingTags_ShouldBeReturnText"),
        new TestCaseData(new List<Token>
            {
                new(@"\", null, TokenType.Escape),
                new("_", null, TokenType.Text),
                new("text", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("text", null, TokenType.Text), " "),
                    TokenType.Tag)
            },
            new List<Token>
            {
                new("", null, TokenType.Escape),
                new("_", null, TokenType.Text),
                new("text", null, TokenType.Text),
                new("_", Tag.CreateTag(TagType.Italic, "_", new Token("text", null, TokenType.Text), ""), TokenType.Tag)
                {
                    Tag = { TagContent = "_", Status = TagStatus.Closing }
                }
            }).SetName("WhenThereIsEscapeTags_ShouldBeRenderEscapeTag"),
        new TestCaseData(new List<Token>
            {
                new("* ", Tag.CreateTag(TagType.Bulleted, "* ", null, "#"), TokenType.Tag),
                new("# ",
                    Tag.CreateTag(TagType.Heading, "# ",
                        new Token("* ", Tag.CreateTag(TagType.Bulleted, "* ", null, "# "), TokenType.Tag), "t"),
                    TokenType.Tag),
                new("text", null, TokenType.Text),
                new("\n", null, TokenType.LineBreaker)
            },
            new List<Token>
            {
                new("* ", Tag.CreateTag(TagType.Bulleted, "* ", null, "#"), TokenType.Tag)
                {
                    Tag = { Status = TagStatus.SelfClosing, TagContent = "<li>" }
                },
                new("# ",
                    Tag.CreateTag(TagType.Heading, "# ",
                        new Token("* ", Tag.CreateTag(TagType.Bulleted, "* ", null, "# "), TokenType.Tag), "t"),
                    TokenType.Tag)
                {
                    Tag = { Status = TagStatus.SelfClosing, TagContent = "<h1>" }
                },
                new("text", null, TokenType.Text),
                new("</h1></li>\n", null, TokenType.LineBreaker)
            }).SetName("WhenThereIsNonPairedTags_ShouldBeReturnTextWithAllTags")
    };

    [TestCaseSource(nameof(ConstructorRendererTokenList))]
    public void HandleTokensTests(List<Token> tokens, List<Token> expectedTokens)
    {
       var renderer = new HtmlRenderer();
        renderer.HandleTokens(tokens).Should().BeEquivalentTo(expectedTokens);
    }
}