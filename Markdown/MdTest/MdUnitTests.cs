using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MdTest
{
    class TestLexer : ILexer
    {
        public Token[] Tokenize()
        {
            return new []{new Token("/", TokenType.TestDelimiter), new Token("/", TokenType.TestDelimiter), };
        }
    }
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
        [TestCase("_hello\t_", "_hello\t_", TestName = "'\\t'InTextDoNotStandOut")]
        [TestCase("_hello\n_", "_hello\n_", TestName = "'\\n'InTextDoNotStandOut")]
        [TestCase("_hello__", "_hello__", TestName = "UnpairedCharactersIsNotTextHighlighting")]
        [TestCase("_italic_simple text_italic_", "<em>italic</em>simple text<em>italic</em>", TestName = "NestingOfTheSameHighlightingCharactersIsIgnored")]
        [TestCase("_!!!_", "<em>!!!</em>", TestName = "PunctuationMarksAreAlsoHighlighted")]

       // [TestCase("__something_is_happend__\t isn't it?__", "<strong>something_is_happend</strong>\nisn't it?__", TestName = "XXX")]
        public void Render_ShouldCorrectConvertToHtml(string inputText, string outputHtmlText)
        {
            Md.Render(inputText).Should().Be(outputHtmlText);
        }
    }
}
