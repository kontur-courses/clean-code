using System.Diagnostics;
using FluentAssertions;
using Markdown;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDown_Tests
    {
        private MarkdownConverter converter = new MarkdownConverter();

        private string markdownTestText = @"Lorem _ipsum dolor sit amet, consectetur adipiscing_ elit. 
Nam ligula sem, __tristique a auctor _et, mollis sit_ amet__ ligula. Fusce lobortis in odio sed sodales. 
Sed at magna maximus nunc dictum tristique ac sed velit. Cras at felis mauris. Aliquam ac nulla vel lacus commodo facilisis non ut purus.
Phasellus aliquet, tellus eget sc__eleris__que mollis, nunc erat rutrum libero, 
sit amet porttitor dui __felis vitae est. Nunc_ condimentum, diam ut rhoncus lacinia, magna dui finibus magna, quis tincidunt enim est vitae mauris. 
Mauris et ipsum non est venenatis dignissim. Nulla justo__ purus, pellentesque ac quam non, vehicula porta nibh.
# Nullam ullamcorper ph__aret__ra lacinia. Curabitur at porta tellus.
Mauris condimentum pretium_ dui, dapibus _consequat quam_ bibendum eget.";

        private string expectedHtmlTestText = @"Lorem <em>ipsum dolor sit amet, consectetur adipiscing</em> elit. 
Nam ligula sem, <strong>tristique a auctor <em>et, mollis sit</em> amet</strong> ligula. Fusce lobortis in odio sed sodales. 
Sed at magna maximus nunc dictum tristique ac sed velit. Cras at felis mauris. Aliquam ac nulla vel lacus commodo facilisis non ut purus.
Phasellus aliquet, tellus eget sc<strong>eleris</strong>que mollis, nunc erat rutrum libero, 
sit amet porttitor dui <strong>felis vitae est. Nunc_ condimentum, diam ut rhoncus lacinia, magna dui finibus magna, quis tincidunt enim est vitae mauris. 
Mauris et ipsum non est venenatis dignissim. Nulla justo</strong> purus, pellentesque ac quam non, vehicula porta nibh.
<h1>Nullam ullamcorper ph<strong>aret</strong>ra lacinia. Curabitur at porta tellus.</h1>
Mauris condimentum pretium_ dui, dapibus <em>consequat quam</em> bibendum eget.";
        private string shortMarkdownTestText = @"Lorem _ipsum dolor sit amet, consectetur adipiscing_ elit. 
Nam ligula sem, __tristique a auctor _et, mollis sit_ amet__ ligula. Fusce lobortis in odio sed sodales. 
Sed at magna maximus nunc dictum tristique ac sed velit. Cras at felis mauris. Aliquam ac nulla vel lacus commodo facilisis non ut purus.";

        private string shortExpectedHtmlTestText = @"Lorem <em>ipsum dolor sit amet, consectetur adipiscing</em> elit. 
Nam ligula sem, <strong>tristique a auctor <em>et, mollis sit</em> amet</strong> ligula. Fusce lobortis in odio sed sodales. 
Sed at magna maximus nunc dictum tristique ac sed velit. Cras at felis mauris. Aliquam ac nulla vel lacus commodo facilisis non ut purus.";

        [TestCase("abc _def_ gh", "abc <em>def</em> gh")]
        [TestCase("abc __def__ gh", "abc <strong>def</strong> gh")]
        [TestCase("# abcdefgh\r\n", "<h1>abcdefgh</h1>\r\n")]
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
        [TestCase(@"asdbsd
# asd
asd", @"asdbsd
<h1>asd</h1>
asd")]
        [TestCase("![](./Images/Image1.jpg)", "<img src=\"./Images/Image1.jpg\">")]
        [TestCase("![](./Images/Image2.jpg)__sometext__",
            "<img src=\"./Images/Image2.jpg\"><strong>sometext</strong>")]
        public void ConverterTests(string markDownText, string expectedResult)
        {
            var htmlText = converter.ConvertToHtml(markDownText);
            htmlText.Should().Be(expectedResult);
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Error)
                TestContext.WriteLine(htmlText);
        }

        [Test]
        public void MarkdownConverter_BigTextTest()
        {
            var htmlText = converter.ConvertToHtml(markdownTestText);
            htmlText.Should().Be(expectedHtmlTestText);
            htmlText = converter.ConvertToHtml(shortMarkdownTestText);
            htmlText.Should().Be(shortExpectedHtmlTestText);
        }

        [Test]
        public void MarkdownConverter_AlgorithmShouldBeLinear()
        {
            var count = 1000;

            var timer = Stopwatch.StartNew();
            for (var i = 0; i < count; i++)
                converter.ConvertToHtml(shortMarkdownTestText);
            timer.Stop();
            var firstTime = timer.ElapsedMilliseconds;

            timer.Restart();
            for (var i = 0; i < count; i++)
                converter.ConvertToHtml(markdownTestText);
            timer.Stop();
            var secondTime = timer.ElapsedMilliseconds;

            var delta = secondTime / (double) firstTime;
            delta.Should().BeLessOrEqualTo(markdownTestText.Length / (double) shortExpectedHtmlTestText.Length * 1.1);
        }
    }
}