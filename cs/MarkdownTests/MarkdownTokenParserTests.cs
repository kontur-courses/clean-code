using FluentAssertions;
using Markdown.Data;
using Markdown.TokenParser;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenParserTests
    {
        private MarkdownTokenParser parser;

        [SetUp]
        public void SetUp()
        {
            var tags = new MarkdownLanguage().GetAllTags;
            parser = new MarkdownTokenParser(tags);
        }

        [Test]
        public void TestGetTokens_OnNull()
        {
            const string inputString = null;
            var expectedResult = new Token[0];
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnEmptyString()
        {
            const string inputString = "";
            var expectedResult = new Token[0];
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetters()
        {
            const string inputString = "abc";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Text, "b"),
                new Token(TokenType.Text, "c")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnDigits()
        {
            var inputString = "123";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "1"),
                new Token(TokenType.Text, "2"),
                new Token(TokenType.Text, "3")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnPunctuationSymbols()
        {
            const string inputString = "!?.,";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "!"),
                new Token(TokenType.Text, "?"),
                new Token(TokenType.Text, "."),
                new Token(TokenType.Text, ",")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnShortTag()
        {
            const string inputString = "_";
            var expectedResult = new[] { new Token(TokenType.Tag, "_") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLongTag()
        {
            const string inputString = "__";
            var expectedResult = new[] { new Token(TokenType.Tag, "__") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading1Tag()
        {
            const string inputString = "# ";
            var expectedResult = new[] { new Token(TokenType.Tag, "# ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading2Tag()
        {
            const string inputString = "## ";
            var expectedResult = new[] { new Token(TokenType.Tag, "## ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading3Tag()
        {
            const string inputString = "### ";
            var expectedResult = new[] { new Token(TokenType.Tag, "### ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading4Tag()
        {
            const string inputString = "#### ";
            var expectedResult = new[] { new Token(TokenType.Tag, "#### ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading5Tag()
        {
            const string inputString = "##### ";
            var expectedResult = new[] { new Token(TokenType.Tag, "##### ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading6Tag()
        {
            const string inputString = "###### ";
            var expectedResult = new[] { new Token(TokenType.Tag, "###### ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnHeading1TagWithoutSpace()
        {
            const string inputString = "#a";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "#"),
                new Token(TokenType.Text, "a")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnOneSpace()
        {
            var inputString = " ";
            var expectedResult = new[] { new Token(TokenType.Space, " ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnOneNewLine()
        {
            const string inputString = "\n";
            var expectedResult = new[] { new Token(TokenType.Tag, "\n") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnEscapeSymbol()
        {
            const string inputString = "\\";
            var expectedResult = new[] { new Token(TokenType.Text, "\\") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnTwoSpaces()
        {
            const string inputString = "  ";
            var expectedResult = new[] { new Token(TokenType.Space, "  ") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnTab()
        {
            const string inputString = "\t";
            var expectedResult = new[] { new Token(TokenType.Space, "\t") };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetterAndSpace()
        {
            const string inputString = "a ";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " ")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnSpaceAndLetter()
        {
            const string inputString = " a";
            var expectedResult = new[]
            {
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "a")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnSpaceBetweenLetters()
        {
            const string inputString = "a b";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Space, " "),
                new Token(TokenType.Text, "b")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnShortTagAndLetter()
        {
            const string inputString = "_a";
            var expectedResult = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnNewLineBetweenLetters()
        {
            const string inputString = "a\nb";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "\n"),
                new Token(TokenType.Text, "b")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetterAndShortTag()
        {
            const string inputString = "a_";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetterBetweenShortTag()
        {
            const string inputString = "_a_";
            var expectedResult = new[]
            {
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLongTagAndLetter()
        {
            const string inputString = "__a";
            var expectedResult = new[]
            {
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "a")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetterAndLongTag()
        {
            const string inputString = "a__";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "__")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetterBetweenLongTag()
        {
            const string inputString = "__a__";
            var expectedResult = new[]
            {
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "__")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnEscapeSymbolAndLetter()
        {
            const string inputString = "\\a";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "\\"),
                new Token(TokenType.Text, "a")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnLetterAndEscapeSymbol()
        {
            const string inputString = "a\\";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Text, "\\")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnBothTags()
        {
            const string inputString = "___";
            var expectedResult = new[]
            {
                new Token(TokenType.Tag, "__"),
                new Token(TokenType.Tag, "_")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnShortTagBetweenSpaces()
        {
            const string inputString = " _ ";
            var expectedResult = new[]
            {
                new Token(TokenType.Space, " "),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Space, " ")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void TestGetTokens_OnShortTagBetweenLetters()
        {
            const string inputString = "a_b";
            var expectedResult = new[]
            {
                new Token(TokenType.Text, "a"),
                new Token(TokenType.Tag, "_"),
                new Token(TokenType.Text, "b")
            };
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }
    }
}