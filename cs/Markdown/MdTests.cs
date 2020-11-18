using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("I'm a _cursive_ :3", "I'm a <em>cursive</em> :3")]
        [TestCase("_I'm a cursive :3_", "<em>I'm a cursive :3</em>")]
        [TestCase("_I'm a_ _cursive :3_", "<em>I'm a</em> <em>cursive :3</em>")]
        public void Should_SupportCursiveTag(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [TestCase("I'm a __bold__ :3", "I'm a <strong>bold</strong> :3")]
        [TestCase("__I'm a bold :3__", "<strong>I'm a bold :3</strong>")]
        [TestCase("__I'm a__ __bold :3__", "<strong>I'm a</strong> <strong>bold :3</strong>")]
        public void Should_SupportBoldTag(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [TestCase("# Header", "<h1>Header</h1>")]
        [TestCase("# Header with many words", "<h1>Header with many words</h1>")]
        public void Should_SupportHeaderTag(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [TestCase("\\# Cat\\s \\\\__are__ \\_cute_", "# Cat\\s \\<strong>are</strong> _cute_")]
        [TestCase(@"\\\\", @"\\\\")] //Так как последний экран ничего не экранирует, то он считается как обычный символ, аналогично остальные
        public void Should_SupportShielding(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [TestCase("Hey i'm covered _part_ial", "Hey i'm covered <em>part</em>ial")]
        [TestCase("Hey i'm covered part_ial_", "Hey i'm covered part<em>ial</em>")]
        [TestCase("Hey i'm covered __part__ial", "Hey i'm covered <strong>part</strong>ial")]
        [TestCase("Hey i'm covered part__ial__", "Hey i'm covered part<strong>ial</strong>")]
        public void Should_SupportPartialCovering_ForOneWord(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [TestCase("__ ignored__", "__ ignored__")]
        [TestCase("_ ignored_", "_ ignored_")]
        [TestCase("# Ha-ha _ we're_ __ ignored__", "<h1>Ha-ha _ we're_ __ ignored__</h1>")]
        public void Should_IgnoreTag_WhenWhiteSpace_AfterOpener(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [TestCase("_ignored _", "_ignored _")]
        [TestCase("__ignored __", "__ignored __")]
        [TestCase("# Ha _gotcha _ :3_ __And __ u2__", "<h1>Ha <em>gotcha _ :3</em> <strong>And __ u2</strong></h1>")]
        public void Should_IgnoreTag_WhenWhiteSpace_BeforeCloser(string input, string expected)
        {
            md.Render(input)
                .Should().Be(expected);
        }

        [Test]
        public void Should_IgnoreEmptyTags()
        {
            md.Render("____ _He is a_ __traitor!__")
                .Should().Be("____ <em>He is a</em> <strong>traitor!</strong>");
        }

        [Test]
        public void Should_IgnoreDifferentTagIntersections()
        {
            md.Render("_Uhm... what __the _f**k__ __is_ ___going_ on__ here?")
                .Should().Be("_Uhm... what __the _f**k__ <strong>is_ __<em>going</em> on</strong> here?");
        }

        [Test]
        public void Should_IgnoreBoldTag_WhenInsideOfCursiveTag()
        {
            md.Render("_Ugh :( __I'm ignored again__ :(_")
                .Should().Be("<em>Ugh :( __I'm ignored again__ :(</em>");
        }

        [Test]
        public void Should_IgnoreTags_WhichCovering_DifferentWordsPartially()
        {
            md.Render("# I'm a pa_rt an_d I'm ignored :(")
                .Should().Be("<h1>I'm a pa_rt an_d I'm ignored :(</h1>");
        }

        [Test]
        public void Should_IgnoreTags_WhereIntersectedNumber()
        {
            md.Render("_12_3 3_2_1 12_3_ _123_ qwe_123_")
                .Should().Be("_12_3 3_2_1 12_3_ <em>123</em> qwe_123_");
        }

        [Test]
        public void Should_BeCloseToLinearEfficiency()
        {
            var sw = new Stopwatch();
            var mdString = "_Tag_ __Bold Tag__ /_not tag_ ";
            var sb = new StringBuilder(mdString);

            md.Render(mdString);

            sw.Start();
            md.Render(mdString);
            sw.Stop();
            var firstTime = sw.ElapsedTicks;

            for (var i = 1; i < 500; i++)
            {
                sb.Append(mdString);
                var markdown = sb.ToString();
                sw.Restart();
                md.Render(markdown);
                sw.Stop();
                var currentTime = sw.ElapsedTicks;
                currentTime
                    .Should().BeLessThan(firstTime * (i + 1) * 2);
            }
        }
    }
}