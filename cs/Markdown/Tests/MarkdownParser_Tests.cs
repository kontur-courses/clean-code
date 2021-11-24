using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownParser_Tests
    {
        private MdParser mdParser;
        
        [SetUp]
        public void SetUp()
        {
            mdParser = new MdParser();
        }
        
        [TestCase("qwerty")]
        public void Parse_ValidText_NotThrows(string text)
        {
            Action parseAction = () => mdParser.Parse(text);
            parseAction.Should().NotThrow();
        }

        [Test]
        public void Parse_SimplePlainText()
        {
            var text = "qwerty";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement("Body", 
                new HyperTextElement("Paragraph", new HyperTextElement("PlainText", "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_SimpleItalicText()
        {
            var text = "_qwerty_";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement("Body", 
                new HyperTextElement("Paragraph", new HyperTextElement("ItalicText", "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_SimpleBoldText()
        {
            var text = "__qwerty__";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement("Body", 
                new HyperTextElement("Paragraph", new HyperTextElement("BoldText", "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_HeaderPlainText()
        {
            var text = "#qwerty";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement("Body", 
                new HyperTextElement("Header", new HyperTextElement("PlainText", "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_MultipleParagraphs()
        {
            var text = "qwerty\r\n_qwerty_";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement("Body",
                new HyperTextElement("Paragraph", new HyperTextElement("PlainText", "qwerty")),
                new HyperTextElement("Paragraph", new HyperTextElement("ItalicText", "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_MultipleTypeTextInParagraph()
        {
            var text = "__qwerty___hello_world";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement("Body", 
                new HyperTextElement("Paragraph", new HyperTextElement("BoldText", "qwerty"),
            new HyperTextElement("ItalicText", "hello"), new HyperTextElement("PlainText", "world")));
            actual.Should().BeEquivalentTo(expected);
        }
    }
}