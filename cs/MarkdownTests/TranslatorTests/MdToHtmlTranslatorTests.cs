using System.Collections.Generic;
using Markdown.Core.Tokens;
using Markdown.Core.Translators;
using NUnit.Framework;

namespace MarkdownTests.TranslatorTests
{
    [TestFixture]
    public class MdToHtmlTranslatorTests
    {
        private readonly MdToHtmlTranslator translator = new MdToHtmlTranslator();

        [TestCaseSource(typeof(MdToHtmlTranslatorTestsData),
            nameof(MdToHtmlTranslatorTestsData.MdToHtmlTranslatorTestCases))]
        public string TranslateTokensToHtmlShouldReturnCorrectHtml(List<Token> tokens)
        {
            return translator.TranslateTokensToHtml(tokens);
        }
    }
}