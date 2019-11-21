using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tools;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    public class TokenizerTests
    {
        private Tokenizer GetTokenizer(string markdown)
        {
            var tags = new List<MarkdownTag> {new BoldTag(), new ItalicTag()};
            var classifier = new CharClassifier(tags.SelectMany(t => t.String));
            
            var tagsReader = new TagsReader(markdown, tags, classifier);
            var events = tagsReader.GetEvents();
            var tokenizer = new Tokenizer(markdown, events, classifier);

            return tokenizer;
        }

        private static IEnumerable<TestCaseData> GetTokensWithInvalidTags()
        {
            yield return new TestCaseData("only plain text",
                    new List<Token> {new Token(TokenType.PlainText, "only plain text")})
                .SetName("not exist any tags");

            yield return new TestCaseData("_text__",
                    new List<Token> {new Token(TokenType.PlainText, "_text__")})
                .SetName("italic start and bold end");

            yield return new TestCaseData("__text_",
                    new List<Token> {new Token(TokenType.PlainText, "__text_")})
                .SetName("bold start and italic end");

            yield return new TestCaseData("_ text_",
                    new List<Token> {new Token(TokenType.PlainText, "_ text_")})
                .SetName("invalid italic start");

            yield return new TestCaseData("_text_t",
                    new List<Token> {new Token(TokenType.PlainText, "_text_t")})
                .SetName("invalid italic end");

            yield return new TestCaseData("__ text_",
                    new List<Token> {new Token(TokenType.PlainText, "__ text_")})
                .SetName("invalid bold start");

            yield return new TestCaseData("_text__t",
                    new List<Token> {new Token(TokenType.PlainText, "_text__t")})
                .SetName("invalid bold end");

            yield return new TestCaseData("_text",
                    new List<Token> {new Token(TokenType.PlainText, "_text")})
                .SetName("italic start without end");

            yield return new TestCaseData("text_",
                    new List<Token> {new Token(TokenType.PlainText, "text_")})
                .SetName("italic end without start");

            yield return new TestCaseData("__text",
                    new List<Token> {new Token(TokenType.PlainText, "__text")})
                .SetName("bold start without end");

            yield return new TestCaseData("text__",
                    new List<Token> {new Token(TokenType.PlainText, "text__")})
                .SetName("bold end without start");
        }

        [TestCaseSource(nameof(GetTokensWithInvalidTags))]
        public void GetTokens_WithInvalidTags_ShouldReturnPlainTextToken(
            string markdown, List<Token> expected)
        {
            var tokenizer = GetTokenizer(markdown);

            var actual = tokenizer.GetTokens();

            actual.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> GetTokensWithValidTagsAndOutsideText()
        {
            yield return new TestCaseData(
                    "start _text_",
                    new List<Token>()
                    {
                        new Token(TokenType.PlainText, "start "),
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.ItalicEnd)
                    })
                .SetName("valid italic tags after plain text");

            yield return new TestCaseData(
                    "_text_ end",
                    new List<Token>()
                    {
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.ItalicEnd),
                        new Token(TokenType.PlainText, " end")
                    })
                .SetName("valid italic tags before plain text");

            yield return new TestCaseData(
                    "sta rt _text_ end .",
                    new List<Token>()
                    {
                        new Token(TokenType.PlainText, "sta rt "),
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.ItalicEnd),
                        new Token(TokenType.PlainText, " end .")
                    })
                .SetName("valid italic tags between plain text");

            yield return new TestCaseData(
                    "start __text__",
                    new List<Token>()
                    {
                        new Token(TokenType.PlainText, "start "),
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.BoldEnd)
                    })
                .SetName("valid bold tags after plain text");

            yield return new TestCaseData(
                    "__text__ end",
                    new List<Token>()
                    {
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.BoldEnd),
                        new Token(TokenType.PlainText, " end")
                    })
                .SetName("valid bold tags before plain text");

            yield return new TestCaseData(
                    "sta rt __text__ end .",
                    new List<Token>()
                    {
                        new Token(TokenType.PlainText, "sta rt "),
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.BoldEnd),
                        new Token(TokenType.PlainText, " end .")
                    })
                .SetName("valid bold tags between plain text");
        }

        [TestCaseSource(nameof(GetTokensWithValidTagsAndOutsideText))]
        public void GetTokens_WithValidTagsAndOutsideText_ShouldReturnRightTokens(
            string markdown, List<Token> expected)
        {
            var tokenizer = GetTokenizer(markdown);

            var actual = tokenizer.GetTokens();

            actual.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> GetTokensWithNestedTags()
        {
            yield return new TestCaseData(
                    "__start _text_ end__",
                    new List<Token>()
                    {
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "start "),
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.ItalicEnd),
                        new Token(TokenType.PlainText, " end"),
                        new Token(TokenType.BoldEnd)
                    })
                .SetName("bold contains text and italic");

            yield return new TestCaseData(
                    "_start __text__ end_",
                    new List<Token>()
                    {
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "start "),
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.BoldEnd),
                        new Token(TokenType.PlainText, " end"),
                        new Token(TokenType.ItalicEnd)
                    })
                .SetName("italic contains text and bold");

            yield return new TestCaseData(
                    "__start __text__ end__",
                    new List<Token>()
                    {
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "start "),
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.BoldEnd),
                        new Token(TokenType.PlainText, " end"),
                        new Token(TokenType.BoldEnd)
                    })
                .SetName("bold contains text and bold");

            yield return new TestCaseData(
                    "_start _text_ end_",
                    new List<Token>()
                    {
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "start "),
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.ItalicEnd),
                        new Token(TokenType.PlainText, " end"),
                        new Token(TokenType.ItalicEnd)
                    })
                .SetName("italic contains text and italic");

            yield return new TestCaseData(
                    "___text___",
                    new List<Token>()
                    {
                        new Token(TokenType.BoldStart),
                        new Token(TokenType.ItalicStart),
                        new Token(TokenType.PlainText, "text"),
                        new Token(TokenType.ItalicEnd),
                        new Token(TokenType.BoldEnd)
                    })
                .SetName("italic contains only bold");
        }

        [TestCaseSource(nameof(GetTokensWithNestedTags))]
        public void GetTokens_WithNestedTags_ShouldReturnRightList(string markdown, List<Token> expected)
        {
            var tokenizer = GetTokenizer(markdown);

            var actual = tokenizer.GetTokens();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}