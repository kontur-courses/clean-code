using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace MdTest
{
    [TestFixture]
    public class MdUnitTests
    { 
        [TestCase("", "", TestName = "EmptyString")]
        [TestCase("abc", "abc", TestName = "SimpleString")]
        [TestCase("_a_", "<em>a</em>", TestName = "ItalicTestHighlightingString")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "BoldTestHighlightingString")]
        [TestCase(@"\__a\__", "__a__", TestName = "CharactersEscaping")]
        [TestCase(@"\__a__", "__a__", TestName = "SingleCharacterEscaping")]
        [TestCase("_italic__bold and italic___", "<em>italic<strong>bold and italic</strong></em>", TestName = "Nesting case: bold text highlighting inside italic")]
        [TestCase("_123_", "_123_", TestName = "OnlyDigitsDoNotStandOut")]
        [TestCase("_1and3_", "<em>1and3</em>", TestName = "DigitsWithOtherCharactersStandOut")]
        [TestCase("_ hello_", "_ hello_", TestName = "SpaceInStartDoNotStandOut")]
        [TestCase("_hello _", "_hello _", TestName = "SpaceInEndDoNotStandOut")]
        [TestCase("_hello__", "_hello__", TestName = "UnpairedCharactersIsNotTextHighlighting")]
        [TestCase("_italic_simple text_italic_", "<em>italic</em>simple text<em>italic</em>", TestName = "NestingOfTheSameHighlightingCharactersIsIgnored")]
        [TestCase("_!!!_", "<em>!!!</em>", TestName = "PunctuationMarksAreAlsoHighlighted")]
        public void Render_ShouldCorrectConvertToHtml(string inputText, string outputHtmlText)
        {
            Md.Render(inputText).Should().Be(outputHtmlText);
        }
    }
}
