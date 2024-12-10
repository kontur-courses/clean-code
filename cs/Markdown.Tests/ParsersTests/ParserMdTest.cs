using FluentAssertions;
using Markdown.Parsers.MdParsers;
using Markdown.Tokens.HtmlTokens;

namespace Markdown.Tests.ParsersTests
{
    [TestFixture]
    public class ParserMdTests
    {
        private ParserMd _parser;
        private readonly List<IRenderable> _expected = new List<IRenderable>();

        [SetUp]
        public void SetUp()
        {
            _parser = new ParserMd();
            _expected.Clear();
        }

        [TestCase("__Bold token__")]
        public void Parse_SimpleBoldText_Correctly(string text)
        {
            _expected.Add(
                new BoldToken(
                    new TextToken("Bold token")));
            CheckCorrectness(text);
        }

        [TestCase("# Header token")]
        public void Parse_SimpleHeaderText_Correctly(string text)
        {
            _expected.Add(
                new HeaderToken(
                    new TextToken("Header token")));
            CheckCorrectness(text);
        }

        [TestCase("_Italic token_")]
        public void Parse_SimpleItalicText_Correctly(string text)
        {
            _expected.Add(
                new ItalicToken(
                    new TextToken("Italic token")));
            CheckCorrectness(text);
        }

        [TestCase("Text Token")]
        public void Parse_SimpleText_Correctly(string text)
        {
            _expected.Add(
                new TextToken(text));
            CheckCorrectness(text);
        }

        [TestCase("# _Set_ __of__ tokens")]
        public void Parse_ComplexText_Correctly(string text)
        {
            _expected.Add(
                new HeaderToken(
                    new SetToken(
                        new ItalicToken(
                            new TextToken("Set")),
                        new TextToken(" "),
                        new BoldToken(
                            new TextToken("of")),
                        new TextToken(" tokens"))));
            CheckCorrectness(text);
        }

        private void CheckCorrectness(string text)
        {
            var actual = _parser.Parse(text);
            actual.Should().BeEquivalentTo(_expected,
                options => options.RespectingRuntimeTypes());
        }
    }
}
