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
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Paragraph, new HyperTextElement<string>(TextType.PlainText, "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_SimpleItalicText()
        {
            var text = "_qwerty_";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement(TextType.ItalicText, 
                        new HyperTextElement<string>(TextType.PlainText, "qwerty"))));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_SimpleBoldText()
        {
            var text = "__qwerty__";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement(TextType.BoldText, 
                        new HyperTextElement<string>(TextType.PlainText, "qwerty"))));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_HeaderPlainText()
        {
            var text = "#qwerty";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Header, new HyperTextElement<string>(TextType.PlainText, "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_MultipleParagraphs()
        {
            var text = "qwerty\r\n_qwerty_";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement<string>(TextType.PlainText, "qwerty")),
                             new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement(TextType.ItalicText,
                         new HyperTextElement<string>(TextType.PlainText, "qwerty"))));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_MultipleTypeTextInParagraph()
        {
            var text = "__qwerty___hello_world";
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement(TextType.BoldText, new HyperTextElement<string>(TextType.PlainText,"qwerty")),
                                 new HyperTextElement(TextType.ItalicText, 
                                     new HyperTextElement<string>(TextType.PlainText, "hello")), 
                                 new HyperTextElement<string>(TextType.PlainText, "world")));
            actual.Should().BeEquivalentTo(expected);
        }
    }
}