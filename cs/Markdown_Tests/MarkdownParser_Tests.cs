using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class MarkdownParser_Tests
    {
        private MdParser mdParser;
        
        [SetUp]
        public void SetUp()
        {
            mdParser = MdParser.Default;
        }

        public static StringWithShielding GetShieldedString(string s)
        {
            return new StringWithShielding(s, Md.ShieldingSymbol, '!',
                new HashSet<char>() { Md.ItalicQuotes, Md.HeaderSymbol });
        }
        
        [TestCase("qwerty")]
        public void Parse_ValidText_NotThrows(string text)
        {
            var shielded = GetShieldedString(text);
            Action parseAction = () => mdParser.Parse(shielded);
            parseAction.Should().NotThrow();
        }

        [Test]
        public void Parse_SimplePlainText()
        {
            var text = GetShieldedString("qwerty");
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Paragraph, new HyperTextElement<string>(TextType.PlainText, "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_SimpleItalicText()
        {
            var text = GetShieldedString("_qwerty_");
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
            var text = GetShieldedString("__qwerty__");
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
            var text = GetShieldedString("# qwerty");
            var actual = mdParser.Parse(text).Value;
            var expected = new HyperTextElement(TextType.Body, 
                new HyperTextElement(TextType.Header, new HyperTextElement<string>(TextType.PlainText, "qwerty")));
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_MultipleParagraphs()
        {
            var text = GetShieldedString("qwerty\r\n_qwerty_");
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
            var text = GetShieldedString("__qwerty___hello_world");
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