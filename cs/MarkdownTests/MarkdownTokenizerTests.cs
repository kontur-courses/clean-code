using FluentAssertions;
using Markdown.Tags;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using NUnit.Framework.Interfaces;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenizerTests
    {
        private MarkdownTokenizer tokenizer;
        private List<IToken> tokens = new();

        [SetUp]
        public void SetUp()
        {
            tokenizer = new MarkdownTokenizer();
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            var i = 0;
            Console.WriteLine("Got token tree:");
            foreach (var token in tokens)
            {
                DrawTokenTree(token, $"[{i}]");
                i++;
            }
        }

        //Basic tokenization 

        [Test]
        public void Tokenize_ShouldReturnTextToken_ForPlainText()
        {
            tokens = tokenizer.Tokenize("This is plain text.");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>();
            tokens[0].TextContent.Should().Be("This is plain text.");
        }

        [Test]
        public void Tokenize_ShouldReturnStrongTagToken_ForStrongText()
        {
            tokens = tokenizer.Tokenize("__strong text__");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<StrongTag>();
            tokens[0].Children![0].TextContent.Should().Be("strong text");
        }

        [Test]
        public void Tokenize_ShouldReturnMixedTokens_ForMixedContent()
        {
            tokens = tokenizer.Tokenize("This is __strong__ and _cursive_ text.");

            tokens.Should().HaveCount(5);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("This is ");
            tokens[1].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<StrongTag>();
            tokens[1].Children![0].TextContent.Should().Be("strong");
            tokens[2].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(" and ");
            tokens[3].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[3].Children![0].TextContent.Should().Be("cursive");
            tokens[4].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(" text.");
        }

        [Test]
        public void Tokenize_ShouldReturnNestedTokens_ForNestedContent()
        {
            tokens = tokenizer.Tokenize("__strong _and cursive_ text__");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<StrongTag>();
            tokens[0].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("strong ");
            tokens[0].Children![1].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[0].Children![1].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should()
                .Be("and cursive");
            tokens[0].Children![2].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(" text");
        }

        //Escape characters

        [Test]
        public void Tokenize_ShouldReturnTextToken_ForEscapedTag()
        {
            tokens = tokenizer.Tokenize(@"\_escaped tag\_");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("_escaped tag_");
        }

        [Test]
        public void Tokenize_ShouldReturnTagToken_ForEscapedEscapeCharacter()
        {
            tokens = tokenizer.Tokenize(@"\\_escaped escape character_");

            tokens.Should().HaveCount(2);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(@"\");
            tokens[1].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[1].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should()
                .Be("escaped escape character");
        }

        [Test]
        public void Tokenize_ShouldNotRemoveEscapeCharacter_WhenItIsNotBeforeTag()
        {
            tokens = tokenizer.Tokenize(@"\not escaped tag\");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(@"\not escaped tag\");
        }

        //Tag interactions

        [Test]
        public void Tokenize_ShouldNotReturnStrongTag_InCursiveTag()
        {
            tokens = tokenizer.Tokenize("_cursive __strong__ text_");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[0].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should()
                .Be("cursive __strong__ text");
        }

        [Test]
        public void Tokenize_ShouldReturnCursiveTag_InStrongTag()
        {
            tokens = tokenizer.Tokenize("__strong _cursive_ text__");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<StrongTag>();
            tokens[0].Children.Should().HaveCount(3);
            tokens[0].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("strong ");
            tokens[0].Children![1].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[0].Children![1].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("cursive");
            tokens[0].Children![2].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(" text");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTags_InsideWordWithNumbers()
        {
            tokens = tokenizer.Tokenize("word w__1__th numb_3_rs");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("word w__1__th numb_3_rs");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTag_ForTagStartingInDifferendWords()
        {
            tokens = tokenizer.Tokenize("cro_ss word t_ag");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("cro_ss word t_ag");
        }

        [Test]
        public void Tokenize_ShouldReturnTag_ForTagContainedWithinWord()
        {
            tokens = tokenizer.Tokenize("cro_ss_ word t__a__g");

            tokens.Should().HaveCount(5);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("cro");
            tokens[1].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[1].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("ss");
            tokens[2].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(" word t");
            tokens[3].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<StrongTag>();
            tokens[3].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("a");
            tokens[4].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("g");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTag_ForUnpairedTag_WithinOneLine()
        {
            tokens = tokenizer.Tokenize("__unpaired_ tags\npaired __tag__");

            tokens.Should().HaveCount(4);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("__unpaired_ tags");
            tokens[1].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<NewLineTag>();
            tokens[2].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("paired ");
            tokens[3].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<StrongTag>();
            tokens[3].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("tag");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTag_ForTagStart_WithoutTrailingNonSpaceCharacter()
        {
            tokens = tokenizer.Tokenize("word_ word_");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("word_ word_");
        }


        [Test]
        public void Tokenize_ShouldNotReturnTag_ForTagEnd_WithoutLeadingNonSpaceCharacter()
        {
            tokens = tokenizer.Tokenize("_word _word word_");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<CursiveTag>();
            tokens[0].Children![0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("word _word word");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTags_ForIntersectingTags()
        {
            tokens = tokenizer.Tokenize("__text_ with intersecting__ tags_");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("__text_ with intersecting__ tags_");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTag_WithEmptyTagContent()
        {
            const string content = "____";
            tokens = tokenizer.Tokenize(content);

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("____");
        }

        //Header

        [Test]
        public void Tokenize_ShouldCorrectlyTokenize_ForHeaderWithNewLine()
        {
            const string content = "#__Header with tag__ and\nnewline";
            tokens = tokenizer.Tokenize(content);

            tokens.Should().HaveCount(2);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<HeaderTag>();
            tokens[0].Children![0].Should().BeOfType<TagToken>().Which.Children[0].TextContent.Should()
                .Be("Header with tag");
            tokens[0].Children![1].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(" and");
            tokens[1].Should().BeOfType<TextToken>().Which.TextContent.Should().Be("newline");
        }

        [Test]
        public void Tokenize_ShouldNotReturnTag_ForHeaderNotAtStartOfLine()
        {
            const string content = "word #header";
            tokens = tokenizer.Tokenize(content);

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TextToken>().Which.TextContent.Should().Be(content);
        }

        //Image

        [Test]
        public void Tokenize_ShouldReturnImageTagToken_ForImage()
        {
            tokens = tokenizer.Tokenize("![alt](image.jpg)");

            tokens.Should().HaveCount(1);
            tokens[0].Should().BeOfType<TagToken>().Which.Tag.Should().BeOfType<ImageTag>();
            ((TagToken)tokens[0]).Attributes["src"].Should().Be("image.jpg");
            ((TagToken)tokens[0]).Attributes["alt"].Should().Be("alt");
        }

        private static void DrawTokenTree(IToken token, string indent)
        {
            switch (token)
            {
                case TextToken textToken:
                    TestContext.Out.WriteLine($"{indent} Text: \"{textToken.TextContent}\"");
                    break;
                case TagToken tagToken:
                    TestContext.Out.WriteLine($"{indent} Tag: {tagToken.Tag.GetType()} {tagToken.TextContent}");
                    break;
            }

            if (token.Children == null) return;
            var i = 0;
            foreach (var child in token.Children)
            {
                DrawTokenTree(child, $"{indent}[{i}]");
                i++;
            }
        }
    }
}