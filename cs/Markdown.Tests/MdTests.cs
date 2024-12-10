using FluentAssertions;
using Markdown.Parsers.MdParsers;
using Markdown.Renderers;
using System.Diagnostics;

namespace Markdown.Tests
{
    internal class MdTests
    {
        [Test]
        public void Md_ThrowsException_ReceivingNullAsIParser()
        {
            Assert.Throws<ArgumentNullException>(() => new Md(null!, new RendererHTML()));
        }

        [Test]
        public void Md_ThrowsException_ReceivingNullAsIRenderer()
        {
            Assert.Throws<ArgumentNullException>(() => new Md(new ParserMd(), null!));
        }

        [TestCase("__Bold token__", "<strong>Bold token</strong>")]
        [TestCase("_Italic token_", "<em>Italic token</em>")]
        [TestCase("# Header token", "<h1>Header token</h1>")]
        [TestCase("Text token", "Text token")]
        [TestCase("# _Set_ __of__ tokens", "<h1><em>Set</em> <strong>of</strong> tokens</h1>")]
        public void Md_RendersCorrectly_SimpleTokens(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase(@"\_\_Text token\_\_", "__Text token__")]
        [TestCase(@"\_Text token\_", "_Text token_")]
        [TestCase(@"\# Text token", "# Text token")]
        [TestCase(@"#\ Text token", "# Text token")]
        public void Md_RendersCorrectly_SimpleEscapedTokens(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase(@"Ste\gosaur\us", @"Ste\gosaur\us")]
        [TestCase(@"\", @"\")]
        [TestCase(@"_Italic \token_", @"<em>Italic \token</em>")]
        public void Md_RendersCorrectly_WhenEscapedNothing(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase(@"\\a", @"\a")]
        [TestCase(@"\\_\\_Text token\\_\\_", @"\<em>\</em>Text token\<em>\</em>")]
        [TestCase(@"\\_Italic token\\_", @"\<em>Italic token\</em>")]
        public void Md_RendersCorrectly_WhenEscapeEscaped(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("__Outer bold _Inner italic part_ outer bold__", "<strong>Outer bold <em>Inner italic part</em> outer bold</strong>")]
        public void Md_RendersCorrectly_ItalicInsideBold(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("_Outer italic __Inner Bold part__ outer Italic_", "<em>Outer italic __Inner Bold part__ outer Italic</em>")]
        public void Md_RendersCorrectly_BoldInsideItalic(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("_Digits 12 3_", "<em>Digits 12 3</em>")]
        [TestCase("Digits_12_3", "Digits_12_3")]
        [TestCase("Digits__12__3", "Digits__12__3")]
        public void Md_RendersCorrectly_DigitsWithUnderscores(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }


        [TestCase("__Sta__rt", "<strong>Sta</strong>rt")]
        [TestCase("_Sta_rt", "<em>Sta</em>rt")]
        [TestCase("S__tar__t", "S<strong>tar</strong>t")]
        [TestCase("S_tar_t", "S<em>tar</em>t")]
        [TestCase("St__art__", "St<strong>art</strong>")]
        [TestCase("St_art_", "St<em>art</em>")]
        public void Md_RendersCorrectly_WordsWithUnderscores(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("Hel_lo, Wor_ld", "Hel_lo, Wor_ld")]
        [TestCase("Hel__lo, Wor__ld", "Hel__lo, Wor__ld")]
        public void Md_RendersCorrectly_UnderscoresInsideDifferentWords(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("_Hello world__", "_Hello world__")]
        [TestCase("__Hello world_", "__Hello world_")]
        public void Md_RendersCorrectly_UnpairedTags(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("_ Hello world_", "_ Hello world_")]
        [TestCase("_ Hello world _", "_ Hello world _")]
        [TestCase("_Hello world _", "_Hello world _")]
        [TestCase("__ Hello world__", "__ Hello world__")]
        [TestCase("__ Hello world __", "__ Hello world __")]
        [TestCase("__Hello world __", "__Hello world __")]
        public void Md_RendersCorrectly_WithSpaceAfterOrBeforeTag(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("_Hello__ _world__", "_Hello__ _world__")]
        [TestCase("__Hello_ __world_", "__Hello_ __world_")]
        public void Md_RendersCorrectly_WithUnderscoreIntersections(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("__", "__")]
        [TestCase("____", "____")]
        public void Md_RendersCorrectly_UnderscoresWithEmptyValue(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase("# Header token 1\n# Header token 2", "<h1>Header token 1</h1><h1>Header token 2</h1>")]
        [TestCase("this is regular sentence # 123", "this is regular sentence # 123")]
        public void Md_RendersCorrectly_Heading(string input, string expected)
        {
            var md = new Md(new ParserMd(), new RendererHTML());
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [TestCase(5, 10, 0.5)]
        public void Md_ShouldWorkInLinearTime(int iterations, int baseIterationSize, double measurementError)
        {
            var testSizes = new int[iterations];
            testSizes[0] = baseIterationSize;
            for (var i = 1; i < iterations; i++)
            {
                testSizes[i] = 2 * testSizes[i - 1];
            }

            var executionTimes = GetExecutionTimes(testSizes);

            for (var i = 1; i < iterations; i++)
            {
                var growthFactor = executionTimes[i] / executionTimes[i - 1];
                Assert.That(growthFactor, Is.LessThanOrEqualTo(2.0 + measurementError));
            }
        }

        private double[] GetExecutionTimes(int[] sizes)
        {
            var results = new double[sizes.Length];
            var md = new Md(new ParserMd(), new RendererHTML());
            md.Render(GenerateText(sizes[^1]));

            for (var i = 0; i < sizes.Length; i++)
            {
                md = new Md(new ParserMd(), new RendererHTML());
                var text = GenerateText(sizes[i]);

                var stopwatch = Stopwatch.StartNew();
                md.Render(text);
                stopwatch.Stop();

                results[i] = stopwatch.Elapsed.TotalMilliseconds;
            }

            return results;
        }

        private string GenerateText(int numberOfRepetitions)
            => string.Concat(
                Enumerable.Repeat(@"__This _is_ a__ simple text \_for\_ crea\ting  complex _test_ __text__.",
                numberOfRepetitions));
    }
}
