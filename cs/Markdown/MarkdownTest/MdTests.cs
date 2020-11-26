using System.IO;
using FluentAssertions;
using Markdown;
using NUnit.Framework;


namespace MarkdownTest
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void Setup()
        {
            md = new Md();
        }

        [Test]
        public void ReturnsText_WhenThereIsNoMarkup()
        {
            var text = "123 dsa 542 asfaasfg";
            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void SingleHeaderTest_ShouldWork()
        {
            var text =
                @"# Header
other text";
            md.MarkdownToHtml(text).Should().Be(@"\<h1>Header\</h1>
other text");
        }

        [Test]
        public void SingleItalicTag_ShouldWork()
        {
            var text = @"some text _italic text_ not italic text";

            md.MarkdownToHtml(text).Should().Be(@"some text \<em>italic text\</em> not italic text");
        }

        [Test]
        public void SingleBoldTag_ShouldWork()
        {
            var text = @"some text __bold text__ not bold text";

            md.MarkdownToHtml(text).Should().Be(@"some text \<strong>bold text\</strong> not bold text");
        }

        [Test]
        public void Tags_ShouldBeInCorrectOrder()
        {
            var text = @"# abc _dsa_";

            md.MarkdownToHtml(text).Should().Be(@"\<h1>abc \<em>dsa\</em>\</h1>");
        }

        [Test]
        public void HeaderTag_ShouldBeFirstSymbolOfParagraph()
        {
            var text = @"abc
vv# dsa";
            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void Italic_ShouldWorkInsideBold()
        {
            var text = @"Внутри __двойного выделения _одинарное_ тоже__ работает.";

            md.MarkdownToHtml(text).Should()
                .Be(@"Внутри \<strong>двойного выделения \<em>одинарное\</em> тоже\</strong> работает.");
        }

        [Test]
        public void Bold_ShouldNotWorkInsideItalic()
        {
            var text = @"Но не наоборот — внутри _одинарного __двойное__ не_ работает.";

            md.MarkdownToHtml(text).Should()
                .Be(@"Но не наоборот — внутри \<em>одинарного __двойное__ не\</em> работает.");
        }

        [Test]
        public void Tag_ShouldNotWork_WhenTagsInDifferentWords()
        {
            var text = @"aaaa bb_ds ds_dsa";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void Tag_ShouldNotWorkInsideNumbers()
        {
            var text = @"_12_3";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void Tags_ShouldBePaired()
        {
            var text = @"text __dsa _dsa";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void OpeningTag_ShouldNotBeFollowedByWhiteSpace()
        {
            var text = @"text_ das_";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void ClosingTag_ShouldNotBePrecededByWhiteSpace()
        {
            var text = @"text _dsa _dasd";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [TestCase(@"abc __ dsa")]
        [TestCase(@"abc ____ dsa")]
        public void StringBetweenTags_ShouldNotBeEmpty(string input)
        {
            md.MarkdownToHtml(input).Should().Be(input);
        }

        [TestCase(@"abc \\_aaa_ aa", @"abc \\<em>aaa\</em> aa")]
        [TestCase(@"abc \_aaa_ aa", @"abc _aaa_ aa")]
        [TestCase(@"abc _aaa\_ aa", @"abc _aaa_ aa")]
        public void TagsEscaping_ShouldWork(string input, string expected)
        {
            md.MarkdownToHtml(input).Should().Be(expected);
        }

        [Test]
        public void MultipleSharpsTags_ShouldNotBecomeTag()
        {
            var text = @"## asd
gfafsa";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [Test]
        public void TripleUnderscore_ShouldNotBecomeTag()
        {
            var text = "123 ___das__";

            md.MarkdownToHtml(text).Should().Be(text);
        }

        [TestCase(@"../../../100000Words.txt"), Timeout(10000)]
        public void Big_PerformanceTest(string dataFileName)
        {
            var text = File.ReadAllText(dataFileName);

            md.MarkdownToHtml(text);
        }

        [TestCase(@"../../../10000Words.txt"), Timeout(1000)]
        public void Medium_PerformanceTest(string dataFileName)
        {
            var text = File.ReadAllText(dataFileName);

            md.MarkdownToHtml(text);
        }

        [TestCase(@"../../../1000Words.txt"), Timeout(100)]
        public void Small_PerformanceTest(string dataFileName)
        {
            var text = File.ReadAllText(dataFileName);

            md.MarkdownToHtml(text);
        }
    }
}