using System.Collections.Generic;
using FluentAssertions;
using Markdown.Core.Normalizer;
using Markdown.Core.Tokens;
using NUnit.Framework;

namespace MarkdownTests.MdNormalizerTests
{
    [TestFixture]
    public class MdNormalizerTests
    {
        private readonly MdNormalizer normalizer = new MdNormalizer();

        [TestCaseSource(typeof(MdNormalizerTestsData), nameof(MdNormalizerTestsData.NormalizerTestCases))]
        public void NormalizeTokensShouldReturnCorrectTokens(List<Token> tokens, List<Token> expectedResult,
            List<IgnoreInsideRule> ignoreInsideRules)
        {
            normalizer.NormalizeTokens(tokens, ignoreInsideRules).Should().Equal(expectedResult);
        }
    }
}