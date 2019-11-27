using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownParserTests
    {
        private readonly MarkdownParser parser = new MarkdownParser(new TokenInfo());

        [TestCase("_", TestName = "Em tag escaped")]
        [TestCase("__", TestName = "Strong tag escaped")]
        public void Parse_ShouldNotParse_WhenEscaped(string mdTag)
        {
            var expectedToken = new RootTokenBuilder()
                .SetData(mdTag + "ba cf" + mdTag)
                .Build();

            var actualToken = parser.Parse($"\\{mdTag}ba cf\\{mdTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldMarkTokenAsInvalid_WhenStrongInsideEm()
        {
            var strongToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag("__")
                .SetHtmlTagName("strong")
                .SetData("bb bb")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var emToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag("_")
                .SetHtmlTagName("em")
                .SetData("aa  aa")
                .SetIsClosed(true)
                .SetIsValid(true)
                .AddNestedToken(strongToken)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .AddNestedToken(emToken)
                .Build();

            var actualToken = parser.Parse("_aa __bb bb__ aa_");

            actualToken.IsValid.Should().Be(expectedToken.IsValid);
        }

        [Test]
        public void Parse_ShouldNotParse_WhenSpaceAfterOpenTag()
        {
            var expectedToken = new RootTokenBuilder()
                .SetData("__ aa aa__")
                .Build();

            var actualToken = parser.Parse("__ aa aa__");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldNotParse_WhenSpaceBeforeCloseTag()
        {
            var expectedToken = new RootTokenBuilder()
                .SetData("__aa aa __ ")
                .Build();

            var actualToken = parser.Parse("__aa aa __ ");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldNotParse_WhenTagsInsideText()
        {
            var expectedToken = new RootTokenBuilder()
                .SetData("aa__bb__cc")
                .Build();

            var actualToken = parser.Parse("aa__bb__cc");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("test", TestName = "word when no tags")]
        [TestCase("test  test", TestName = "words when no tags")]
        [TestCase("     test  test    ", TestName = "words with edge spaces when no tags")]
        public void Parse_ShouldParse(string markdown)
        {
            // markdown is equal to token data here
            var expectedToken = new RootTokenBuilder()
                .SetData(markdown)
                .Build();

            var actualToken = parser.Parse(markdown);

            expectedToken.Should().BeEquivalentTo(actualToken);
        }

        [TestCase("_", "em", TestName = "Em tag")]
        [TestCase("__", "strong", TestName = "Strong tag")]
        [TestCase("`", "code", TestName = "Code tag")]
        public void Parse_ShouldParse_WhenSingleNotNested(string mdTag, string htmlTag)
        {
            var nestedToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag(mdTag)
                .SetHtmlTagName(htmlTag)
                .SetData("test tt")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .AddNestedToken(nestedToken)
                .Build();

            var actualToken = parser.Parse($"{mdTag}test tt{mdTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("_", "em", TestName = "Em tags")]
        [TestCase("__", "strong", TestName = "Strong tags")]
        public void Parse_ShouldParse_WhenMultipleNotNested(string mdTag, string htmlTag)
        {
            var firstNestedToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag(mdTag)
                .SetHtmlTagName(htmlTag)
                .SetData("test t")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var secondNestedToken = new TokenBuilder()
                .SetPosition(1)
                .SetMdTag(mdTag)
                .SetHtmlTagName(htmlTag)
                .SetData("abc d")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .SetData(" ")
                .AddNestedToken(firstNestedToken)
                .AddNestedToken(secondNestedToken)
                .Build();

            var actualToken = parser.Parse($"{mdTag}test t{mdTag} {mdTag}abc d{mdTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldParse_WhenMultipleNotNestedEmAndStrongTags()
        {
            var firstNestedToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag("_")
                .SetHtmlTagName("em")
                .SetData("test t")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var secondNestedToken = new TokenBuilder()
                .SetPosition(1)
                .SetMdTag("__")
                .SetHtmlTagName("strong")
                .SetData("abc c")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .SetData(" ")
                .AddNestedToken(firstNestedToken)
                .AddNestedToken(secondNestedToken)
                .Build();

            var actualToken = parser.Parse("_test t_ __abc c__");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldParse_WhenEmInsideStrong()
        {
            var emToken = new TokenBuilder()
                .SetPosition(3)
                .SetMdTag("_")
                .SetHtmlTagName("em")
                .SetData("bb bb")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var strongToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag("__")
                .SetHtmlTagName("strong")
                .SetData("aa  aa")
                .SetIsClosed(true)
                .SetIsValid(true)
                .AddNestedToken(emToken)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .AddNestedToken(strongToken)
                .Build();

            var actualToken = parser.Parse("__aa _bb bb_ aa__");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("#", "h1", TestName = "First level header")]
        [TestCase("##", "h2", TestName = "Second level header")]
        [TestCase("###", "h3", TestName = "Third level header")]
        [TestCase("####", "h4", TestName = "Fourth level header")]
        [TestCase("#####", "h5", TestName = "Fifth level header")]
        public void Parse_ShouldParseHeader(string tag, string htmlTag)
        {
            var headerToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag(tag)
                .SetHtmlTagName(htmlTag)
                .SetData("header test")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .AddNestedToken(headerToken)
                .Build();

            var actualToken = parser.Parse($"{tag} header test\n");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase(">", "\n\n", TestName = "Blockquote without new line")]
        [TestCase("#", "\n\n", TestName = "Header without new line")]
        [TestCase("-", "\n\n", TestName = "List without new line")]
        public void Parse_ShouldNotParse_WhenNoNewLine(string openTag, string closeTag)
        {
            var expectedToken = new RootTokenBuilder()
                .SetData($"test {openTag} should not parse")
                .Build();

            var actualToken = parser.Parse($"test {openTag} should not parse{closeTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase(">", "\n\n", TestName = "Blockquote no space after open tag")]
        [TestCase("#", "\n", TestName = "Header no space after open tag")]
        [TestCase("-", "\n", TestName = "List no space after open tag")]
        public void Parse_ShouldNotParse_WhenNoSpaceAfterOpenTag(string openTag, string closeTag)
        {
            var expectedToken = new RootTokenBuilder()
                .SetData($"{openTag}should not parse")
                .Build();

            var actualToken = parser.Parse($"{openTag}should not parse{closeTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase(">", "\n\n", "blockquote", TestName = "Blockquote ignores start spaces")]
        [TestCase("#", "\n", "h1", TestName = "Header ignores start spaces")]
        public void Parse_ShouldIgnoreSpacesAfterOpenTag(string openTag, string closeTag, string htmlTag)
        {
            var nestedToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag(openTag)
                .SetHtmlTagName(htmlTag)
                .SetData("nested token data")
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .AddNestedToken(nestedToken)
                .Build();

            var actualToken = parser.Parse($"{openTag}     nested token data{closeTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("```", "code", "pre", "m\nn", "```m\nn```", TestName = "Multiline code")]
        [TestCase("-", "li", "ul", "l l", "- l l\n", TestName = "List")]
        public void Parse_ShouldExtraWrap(string mdTag, string htmlTag, string wrapTag, string data, string markdown)
        {
            var nestedToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag(mdTag)
                .SetHtmlTagName(htmlTag)
                .SetData(data)
                .SetIsClosed(true)
                .SetIsValid(true)
                .Build();
            var wrappingToken = new TokenBuilder()
                .SetPosition(0)
                .SetMdTag("")
                .SetHtmlTagName(wrapTag)
                .SetIsClosed(true)
                .SetIsValid(true)
                .AddNestedToken(nestedToken)
                .Build();
            var expectedToken = new RootTokenBuilder()
                .AddNestedToken(wrappingToken)
                .Build();

            var actualToken = parser.Parse(markdown);

            actualToken.Should().BeEquivalentTo(expectedToken);
        }
    }
}