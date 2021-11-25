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
        [TestCase("qwerty_")]
        [TestCase("qwerty__")]
        [TestCase("")]
        [TestCase("1")]
        public void Parse_ValidText_NotThrowsAndSuccess(string text)
        {
            var shielded = GetShieldedString(text);
            ParsingResult result = null;
            Action parseAction = () => result = mdParser.Parse(shielded);
            parseAction.Should().NotThrow();
            result.IsSuccess.Should().BeTrue();
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

        [Test]
        public void Parse_ShieldedItalicQuotes_PlainText()
        {
            var text = GetShieldedString("asd\\_asd  _ ");
            var actual = mdParser.Parse(text).Value;
            var expected =  new HyperTextElement(TextType.Body,
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement<string>(TextType.PlainText, "asd_asd  _")));
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public void Parse_ShieldedBoldQuotes()
        {
            var text = GetShieldedString("asd\\__asd  f__ ");
            var actual = mdParser.Parse(text).Value;
            var expected =  new HyperTextElement(TextType.Body,
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement<string>(TextType.PlainText, "asd_"),
                                 new HyperTextElement(TextType.ItalicText, 
                                     new HyperTextElement<string>(TextType.PlainText, "asd  f")),
                                 new HyperTextElement<string>(TextType.PlainText, "_ ")));
            actual.Should().BeEquivalentTo(expected, 
                config => config.AllowingInfiniteRecursion());
        }

        [Test]
        public void Parse_ShieldedShielding()
        {
            var text = GetShieldedString("_Hi\\\\_");
            var actual = mdParser.Parse(text).Value;
            var expected =  new HyperTextElement(TextType.Body,
                new HyperTextElement(TextType.Paragraph, 
                    new HyperTextElement(TextType.ItalicText, 
                        new HyperTextElement<string>(TextType.PlainText, "Hi\\"))));
            actual.Should().BeEquivalentTo(expected);
        }
    }
}