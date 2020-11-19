using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class MarkdownTests
    {
        [TestCase("I'm a _cursive_ :3", "I'm a <em>cursive</em> :3")]
        [TestCase("_I'm a cursive :3_", "<em>I'm a cursive :3</em>")]
        [TestCase("_I'm a_ _cursive :3_", "<em>I'm a</em> <em>cursive :3</em>")]
        public void Should_SupportCursiveTag(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("I'm a __bold__ :3", "I'm a <strong>bold</strong> :3")]
        [TestCase("__I'm a bold :3__", "<strong>I'm a bold :3</strong>")]
        [TestCase("__I'm a__ __bold :3__", "<strong>I'm a</strong> <strong>bold :3</strong>")]
        public void Should_SupportBoldTag(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("# Header", "<h1>Header</h1>")]
        [TestCase("# Header with many words", "<h1>Header with many words</h1>")]
        [TestCase("This is # not header", "This is # not header")]
        public void Should_SupportHeaderTag(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("\\# Cat\\s \\\\__are__ \\_cute_", "# Cat\\s \\<strong>are</strong> _cute_")]
        [TestCase(@"\\\\", @"\\\\")] //Так как последний экран ничего не экранирует, то он считается как обычный символ, аналогично остальные
        public void Should_SupportShielding(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("Hey i'm covered _part_ial", "Hey i'm covered <em>part</em>ial")]
        [TestCase("Hey i'm covered part_ial_", "Hey i'm covered part<em>ial</em>")]
        [TestCase("Hey i'm covered __part__ial", "Hey i'm covered <strong>part</strong>ial")]
        [TestCase("Hey i'm covered part__ial__", "Hey i'm covered part<strong>ial</strong>")]
        public void Should_SupportPartialCovering_ForOneWord(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("__ ignored__", "__ ignored__")]
        [TestCase("_ ignored_", "_ ignored_")]
        [TestCase("# Ha-ha _ we're_ __ ignored__", "<h1>Ha-ha _ we're_ __ ignored__</h1>")]
        public void Should_IgnoreTag_WhenWhiteSpace_AfterOpener(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("_ignored _", "_ignored _")]
        [TestCase("__ignored __", "__ignored __")]
        [TestCase("# Ha _gotcha _ :3_ __And __ u2__", "<h1>Ha <em>gotcha _ :3</em> <strong>And __ u2</strong></h1>")]
        public void Should_IgnoreTag_WhenWhiteSpace_BeforeCloser(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [Test]
        public void Should_IgnoreEmptyTags()
        {
            MdRenderer.Render("____ _He is a_ __traitor!__")
                .Should().Be("____ <em>He is a</em> <strong>traitor!</strong>");
        }

        [Test]
        public void Should_IgnoreDifferentTagIntersections()
        {
            MdRenderer.Render("_Uhm... what __the _f**k__ __is_ ___going_ on__ here?")
                .Should().Be("_Uhm... what __the _f**k__ <strong>is_ __<em>going</em> on</strong> here?");
        }

        [Test]
        public void Should_IgnoreBoldTag_WhenInsideOfCursiveTag()
        {
            MdRenderer.Render("_Ugh :( __I'm ignored again__ :(_")
                .Should().Be("<em>Ugh :( __I'm ignored again__ :(</em>");
        }

        [TestCase("part_ial cover_", "part_ial cover_")]
        [TestCase("part_ial cov_er", "part_ial cov_er")]
        [TestCase("_partial c_over", "_partial c_over")]
        [TestCase("# I'm a pa_rt an_d I'm ignored :(", "<h1>I'm a pa_rt an_d I'm ignored :(</h1>")]
        public void Should_IgnoreTags_WhichCovering_DifferentWordsPartially(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [TestCase("_12_3", "_12_3")]
        [TestCase("__12__3", "__12__3")]
        [TestCase("1_2_3", "1_2_3")]
        [TestCase("1__2__3", "1__2__3")]
        [TestCase("12_3_", "12_3_")]
        [TestCase("12__3__", "12__3__")]
        [TestCase("123_qwe_", "123_qwe_")]
        [TestCase("123__qwe__", "123__qwe__")]
        [TestCase("_123_", "<em>123</em>")]
        [TestCase("_123qwe_", "<em>123qwe</em>")]
        [TestCase("__123__", "<strong>123</strong>")]
        [TestCase("__123qwe__", "<strong>123qwe</strong>")]
        public void Should_IgnoreTags_WhichIntersectedWordWithNumber(string input, string expected)
        {
            MdRenderer.Render(input)
                .Should().Be(expected);
        }

        [Test]
        public void Should_BeCloseToLinearEfficiency()
        {
            var sw = new Stopwatch();
            var mdString = "_Tag_ __Bold Tag__ /_not tag_ ";
            var sb = new StringBuilder(mdString);

            MdRenderer.Render(mdString);

            sw.Start();
            MdRenderer.Render(mdString);
            sw.Stop();
            var firstTime = sw.ElapsedTicks;

            for (var i = 1; i < 500; i++)
            {
                sb.Append(mdString);
                var markdown = sb.ToString();
                sw.Restart();
                MdRenderer.Render(markdown);
                sw.Stop();
                var currentTime = sw.ElapsedTicks;
                var expectedTime = firstTime * (i + 1);
                currentTime
                    .Should().BeLessOrEqualTo(expectedTime + expectedTime / 2);
            }
        }
    }
}