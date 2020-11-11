using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class TextAnalyzerTests
    {
        [TestCase("#a", new[] {"a"}, new[] {Styles.Title}, TestName = "GetTokens_WhenOneTitle")]
        [TestCase("\\#a", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenEscapedMarkdown")]
        public void GetTokens_WhenOnlyTitlesMarkdowns(string text, string[] expectedValues, Styles[] expectedStyles)
        {
            var actualTokens = MarkdownTextAnalyzer.GetTokens(text).ToArray();
            actualTokens.Length.Should().Be(expectedValues.Length);
            for (var i = 0; i < actualTokens.Length; i++)
            {
                actualTokens[i].Value.Should().Be(expectedValues[i]);
                actualTokens[i].Style.Should().Be(expectedStyles[i]);
            }
        }

        [TestCase("_a_", new[] {"a"}, new[] {Styles.Italic}, TestName = "GetTokens_WhenThereIsOneItalicField")]
        [TestCase("_a_ _b_", new[] {"a", "b"}, new[] {Styles.Italic, Styles.Italic},
            TestName = "GetTokens_WhenThereAreSeveralItalicField")]
        [TestCase("_a", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenThereIsNoClosingMarkdown")]
        [TestCase("_ab_c", new[] {"ab"}, new[] {Styles.Italic}, TestName = "GetTokens_WhenMarkdownsSelectPartOfWord")]
        [TestCase("a_b c_d", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenMarkdownsInDifferentWords")]
        [TestCase("_ ac_", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenWhitespaceAfterOpeningMarkdown")]
        [TestCase("_ac _", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenWhitespaceBeforeClosingMarkdown")]
        [TestCase("\\_a_", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenEscapedMarkdown")]
        [TestCase("_12_", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenMarkdownSelectDigits")]
        public void GetTokens_WhenOnlyItalicMarkdowns(string text, string[] expectedValues, Styles[] expectedStyles)
        {
            var actualTokens = MarkdownTextAnalyzer.GetTokens(text).ToArray();
            actualTokens.Length.Should().Be(expectedValues.Length);
            for (var i = 0; i < actualTokens.Length; i++)
            {
                actualTokens[i].Value.Should().Be(expectedValues[i]);
                actualTokens[i].Style.Should().Be(expectedStyles[i]);
            }
        }

        [TestCase("__a__", new[] {"a"}, new[] {Styles.Bold}, TestName = "GetTokens_WhenThereIsOneBoldField")]
        [TestCase("__a__ __b__", new[] {"a", "b"}, new[] {Styles.Bold, Styles.Bold},
            TestName = "GetTokens_WhenThereAreSeveralBoldField")]
        [TestCase("__a", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenThereIsNoClosingMarkdown")]
        [TestCase("__ab__c", new[] {"ab"}, new[] {Styles.Bold}, TestName = "GetTokens_WhenMarkdownsSelectPartOfWord")]
        [TestCase("a__b c__d", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenMarkdownsInDifferentWords")]
        [TestCase("__ ac__", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenWhitespaceAfterOpeningMarkdown")]
        [TestCase("__ac __", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenWhitespaceBeforeClosingMarkdown")]
        [TestCase("__12__", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenMarkdownSelectDigits")]
        public void GetTokens_WhenOnlyBoldMarkdowns(string text, string[] expectedValues, Styles[] expectedStyles)
        {
            var actualTokens = MarkdownTextAnalyzer.GetTokens(text).ToArray();
            actualTokens.Length.Should().Be(expectedValues.Length);
            for (var i = 0; i < actualTokens.Length; i++)
            {
                actualTokens[i].Value.Should().Be(expectedValues[i]);
                actualTokens[i].Style.Should().Be(expectedStyles[i]);
            }
        }

        [TestCase("#__a__ _b_", new[] {"__a__ _b_", "a", "b"}, new[] {Styles.Title, Styles.Bold, Styles.Italic},
            TestName = "GetTokens_WhenThereAllCorrectMarkdowns")]
        [TestCase("__a_b__c_", new string[0], new Styles[0],
            TestName = "GetTokens_ReturnEmptyCollection_WhenIntersectionOfItalicAndBoldMarkdowns")]
        [TestCase("_a __b__ c_", new[] {"a __b__ c"}, new[] {Styles.Italic},
            TestName = "GetTokens_ReturnOnlyItalicToken_WhenBoldInsideItalicMarkdowns")]
        [TestCase("__a _b_ c__", new[] {"b", "a _b_ c"}, new[] {Styles.Italic, Styles.Bold},
            TestName = "GetTokens_WhenMarkdownsSelectPartOfWord")]
        public void GetTokens_WhenInteractionOfMarkdowns(string text, string[] expectedValues, Styles[] expectedStyles)
        {
            var actualTokens = MarkdownTextAnalyzer.GetTokens(text).ToArray();
            actualTokens.Length.Should().Be(expectedValues.Length);
            for (var i = 0; i < actualTokens.Length; i++)
            {
                actualTokens[i].Value.Should().Be(expectedValues[i]);
                actualTokens[i].Style.Should().Be(expectedStyles[i]);
            }
        }
    }
}