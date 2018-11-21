using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using Markdown;
using FluentAssertions;
using FluentAssertions.Common;

namespace MarkdownTests
{
    
    [TestFixture]
    public class MdTests
    {
        private Random rnd = new Random();

        [TestCase("_abc_", @"<em>abc</em>", TestName = "Tagged italic word")]
        [TestCase("__abc__", @"<strong>abc</strong>", TestName = "Tagged strong word")]
        [TestCase("aa _bb_ aa", @"aa <em>bb</em> aa", TestName = "Tagged word between words")]
        [TestCase("_abc_ d _abc_", @"<em>abc</em> d <em>abc</em>", TestName = "Word between tagged words")]
        [TestCase(@"\_asd\_", "_asd_", TestName = "Escape both tags")]
        [TestCase(@" \_ ", " _ ", TestName = "Escape one symbol")]
        [TestCase(@"_\_a_", @"<em>_a</em>", TestName = "Escape half of tag")]
        [TestCase(@"_abc_ __abc__", @"<em>abc</em> <strong>abc</strong>", TestName = "Italic and strong")]
        [TestCase(@"__abc__ _abc_", @"<strong>abc</strong> <em>abc</em>", TestName = "Strong and italic")]
        [TestCase(@"__abc _cde_ abc__", @"<strong>abc <em>cde</em> abc</strong>", TestName = "Italic in strong")]
        [TestCase(@"__abc _cde_ _cde_ abc__", @"<strong>abc <em>cde</em> <em>cde</em> abc</strong>", TestName = "Two italic in strong")]
        [TestCase(@"_abc __cde__ abc_", @"<em>abc __cde__ abc</em>", TestName = "Strong in italic")]
        [TestCase(@"_abc __cde__ __cde__ abc_", @"<em>abc __cde__ __cde__ abc</em>", TestName = "Two strong in italic")]
        [TestCase("_a", "_a", TestName = "Not closed tag")]
        [TestCase("_a __b", "_a __b", TestName = "Not closed tags")]
        [TestCase("_a __abc__", @"_a <strong>abc</strong>", TestName = "Not closed before closed")]
        [TestCase("_a __a a_", "<em>a __a a</em>", TestName = "Not closed in closed")]
        [TestCase("a_ a_", "a_ a_", TestName = "No whitespace after tag open")]
        [TestCase("_a _a", "_a _a", TestName = "No whitespace before tag close")]
        [TestCase("a_ _a", "a_ _a")]
        [TestCase("_3", "_3", TestName = "Tag before number")]
        [TestCase("1_", "1_", TestName = "Tag after number")]
        [TestCase("abc_12_3", "abc_12_3", TestName = "Tags between numbers")]
        [TestCase("___abc___", "<strong>_abc</strong>_", TestName = "Tags in a row")]
        [TestCase("___a_b___", "<strong><em>a</em>b</strong>_", TestName = "Tags in the row 2")]
        [TestCase("__abc______abc _ _ _ ___a_b___", "<strong>abc</strong><strong></strong>abc _ _ _ <strong><em>a</em>b</strong>_", TestName = "Pre-hard case")]
        [TestCase("___abc______abc _ _ _ ___a_b___", "<strong>_abc</strong><strong></strong>abc _ _ _ <strong><em>a</em>b</strong>_", TestName = "Hard case")]
        [TestCase(@"\\\\", @"\\", TestName = "Escaped backslash")]
        [TestCase(@"\d\\f_dfs\\\fgfgdf___dsf__S__ds____d_\fs___D\__df_s\\\______\_\_D_s\___", @"d\f<em>dfs\fgfgdf</em><strong>dsf</strong>S<strong>ds</strong><strong>d_fs</strong><em>D_</em>df<em>s\_</em><strong></strong>__D<em>s_</em>_", TestName = "Hard case with backslashes")]
        public void ParserShould(string rawString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rawString);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void MarkdownParserWorkTime_ShouldBe_Linear(int length)
        {
            #region string creation
            var alphabet = "()_qwertyuioopasdfgjkl ";
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                if (rnd.Next(100) > 90)
                    builder.Append(GetRandomChar(alphabet));
                else
                {
                    builder.Append("__");
                }
            }

            var rawString = builder.ToString();
            #endregion

            var parser = new MarkdownParser();
            var stopwatch = Stopwatch.StartNew();
            parser.Parse(rawString);
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(rawString.Length);
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void HtmlConverterWorkTime_ShouldBe_Linear(int length)
        {
            #region string creation
            var alphabet = "()_qwertyuioopasdfgjkl ";
            var builder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                if (rnd.Next(100) > 90)
                    builder.Append(GetRandomChar(alphabet));
                else
                {
                    builder.Append("__");
                }
            }

            var rawString = builder.ToString();
            #endregion
            
            var parser = new MarkdownParser();
            var converter = new HtmlConverter();
            var parsedSpan = parser.Parse(rawString);
            var stopwatch = Stopwatch.StartNew();
            converter.Convert(rawString, parsedSpan);
            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(rawString.Length);
        }

        public char GetRandomChar(string alphabet)
        {
            return alphabet[rnd.Next(alphabet.Length)];
        }

        [Test]
        public void HtmlConverter_ShouldConvertCorrectly()
        {
            var converter = new HtmlConverter();
            var span = new Span(new Tag(TagType.Italic, "_", "_"), 0, 2);
            var result = converter.Convert("_a_", span);
            result.Should().BeEquivalentTo("<em>a</em>");
        }
    }
}
