using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MdTest
{
    [TestFixture]
    public class MdUnitTests
    {
        private IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData("", "").SetName("EmptyString");
            yield return new TestCaseData("abc", "abc").SetName("SimpleString");
            yield return new TestCaseData("_a_", "<em>a</em>").SetName("ItalicTestHighlightingString");
            yield return new TestCaseData("__a__", "<strong>a</strong>").SetName("BoldTestHighlightingString");
            yield return new TestCaseData(@"\__a\__", "__a__").SetName("CharactersEscaping");
            yield return new TestCaseData(@"\__a__", "__a__").SetName("SingleCharacterEscaping");
            yield return new TestCaseData("_italic__bold and italic___", "<strong>italic<em>bold and italic</em></strong>")
                .SetName("Nesting case: bold text highlighting inside italic");
            yield return new TestCaseData("__bold_bold and italic___", "<em>italic<strong>bold and italic</strong></em>")
                .SetName("Nesting case: italic text highlighting inside bold");
            yield return new TestCaseData("_123_", "_123_").SetName("OnlyDigitsDoNotStandOut");
            yield return new TestCaseData("_1and3_", "<em>1and3</em>").SetName("DigitsWithOtherCharactersStandOut");
            yield return new TestCaseData("_ hello_", "_ hello_").SetName("SpaceInStartDoNotStandOut");
            yield return new TestCaseData("_hello _", "_ hello_").SetName("SpaceInEndDoNotStandOut");
            yield return new TestCaseData("_hello__", "_hello__").SetName("UnpairedCharactersIsNotTextHighlighting");
            yield return new TestCaseData("_italic_simple text_italic_", "<em>italic</em>simple text<em>italic</em>")
                .SetName("NestingOfTheSameHighlightingCharactersIsIgnored");
            yield return new TestCaseData("_!!!_", "<em>!!!</em>").SetName("PunctuationMarksAreAlsoHighlighted");
        }

        [TestCase(), TestCaseSource(nameof(GetTestCases))]
        public void Render_ShouldCorrectConvertToHtml(string inputText, string outputHtmlText)
        {
            Md.Render(inputText).Should().Be(outputHtmlText);
        }
    }
}
