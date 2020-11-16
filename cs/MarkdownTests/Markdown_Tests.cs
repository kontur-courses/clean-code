using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDown_Tests
    {
        private MarkdownConverter converter = new MarkdownConverter();

        [TestCase("abc _def_ gh", "abc <em>def</em> gh")]
        [TestCase("abc __def__ gh", "abc <strong>def</strong> gh")]
        [TestCase("# abcdefgh\n", "<h1>abcdefgh</h1>\n")]
        [TestCase(@"abc \_ def \_", @"abc _ def _")]
        [TestCase(@"abc \\ def \\", @"abc \ def \")]
        [TestCase(@"abc \ def \", @"abc \ def \")]
        [TestCase(@"abc __def _gh_ ijk__ lmn", @"abc <strong>def <em>gh</em> ijk</strong> lmn")]
        [TestCase(@"abc _def __gh__ ijk_ lmn", @"abc <em>def __gh__ ijk</em> lmn")]
        [TestCase("abc as_12_asd asd", "abc as_12_asd asd")]
        [TestCase("abc as_asd_asd asd", "abc as<em>asd</em>asd asd")]
        [TestCase("abc as_asd a_sd asd", "abc as_asd a_sd asd")]
        [TestCase("abc\nabc as__asd a_sd asd\n", "abc\nabc as__asd a_sd asd\n")]
        [TestCase("abc _ def_ gh", "abc _ def_ gh")]
        [TestCase("abc _def _gh_", "abc <em>def _gh</em>")]
        public void ConverterTests(string markDownText, string expectedResult)
        {
            var htmlText = converter.ConvertToHtml(markDownText);
            htmlText.Should().Be(expectedResult);
        }
    }
}