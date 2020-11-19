using System.Diagnostics;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class MarkdownTests
    {
        [TestCase("I'm a _cursive_ :3", ExpectedResult = "I'm a <em>cursive</em> :3")]
        [TestCase("_I'm a cursive :3_", ExpectedResult = "<em>I'm a cursive :3</em>")]
        [TestCase("_I'm a_ _cursive :3_", ExpectedResult = "<em>I'm a</em> <em>cursive :3</em>")]
        public string Should_SupportCursiveTag(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("I'm a __bold__ :3", ExpectedResult = "I'm a <strong>bold</strong> :3")]
        [TestCase("__I'm a bold :3__", ExpectedResult = "<strong>I'm a bold :3</strong>")]
        [TestCase("__I'm a__ __bold :3__", ExpectedResult = "<strong>I'm a</strong> <strong>bold :3</strong>")]
        public string Should_SupportBoldTag(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("# Header", ExpectedResult = "<h1>Header</h1>")]
        [TestCase("# Header with many words", ExpectedResult = "<h1>Header with many words</h1>")]
        [TestCase("This is # not header", ExpectedResult = "This is # not header")]
        public string Should_SupportHeaderTag(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("\\# Cat\\s \\\\__are__ \\_cute_", ExpectedResult = "# Cat\\s \\<strong>are</strong> _cute_")]
        [TestCase(@"\\\\", ExpectedResult = @"\\\\")] //Так как последний экран ничего не экранирует, то он считается как обычный символ, аналогично остальные
        public string Should_SupportShielding(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("Hey i'm covered _part_ial", ExpectedResult = "Hey i'm covered <em>part</em>ial")]
        [TestCase("Hey i'm covered part_ial_", ExpectedResult = "Hey i'm covered part<em>ial</em>")]
        [TestCase("Hey i'm covered __part__ial", ExpectedResult = "Hey i'm covered <strong>part</strong>ial")]
        [TestCase("Hey i'm covered part__ial__", ExpectedResult = "Hey i'm covered part<strong>ial</strong>")]
        public string Should_SupportPartialCovering_ForOneWord(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("__ ignored__", ExpectedResult = "__ ignored__")]
        [TestCase("_ ignored_", ExpectedResult = "_ ignored_")]
        [TestCase("# Ha-ha _ we're_ __ ignored__", ExpectedResult = "<h1>Ha-ha _ we're_ __ ignored__</h1>")]
        public string Should_IgnoreTag_WhenWhiteSpace_AfterOpener(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("_ignored _", ExpectedResult = "_ignored _")]
        [TestCase("__ignored __", ExpectedResult = "__ignored __")]
        [TestCase("# Ha _gotcha _ :3_ __And __ u2__", ExpectedResult = "<h1>Ha <em>gotcha _ :3</em> <strong>And __ u2</strong></h1>")]
        public string Should_IgnoreTag_WhenWhiteSpace_BeforeCloser(string input)
        {
            return MdRenderer.Render(input);
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
            MdRenderer.Render("__Many _intersected __tags_ here__ :)__")
                .Should().Be("<strong>Many _intersected __tags_ here__ :)</strong>");
        }

        [Test]
        public void Should_IgnoreBoldTag_WhenInsideOfCursiveTag()
        {
            MdRenderer.Render("_Ugh :( __I'm ignored again__ :(_")
                .Should().Be("<em>Ugh :( __I'm ignored again__ :(</em>");
        }

        [TestCase("part_ial cover_", ExpectedResult = "part_ial cover_")]
        [TestCase("part_ial cov_er", ExpectedResult = "part_ial cov_er")]
        [TestCase("_partial c_over", ExpectedResult = "_partial c_over")]
        [TestCase("# I'm a pa_rt an_d I'm ignored :(", ExpectedResult = "<h1>I'm a pa_rt an_d I'm ignored :(</h1>")]
        public string Should_IgnoreTags_WhichCovering_DifferentWordsPartially(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("_12_3", ExpectedResult = "_12_3")]
        [TestCase("__12__3", ExpectedResult = "__12__3")]
        [TestCase("1_2_3", ExpectedResult = "1_2_3")]
        [TestCase("1__2__3", ExpectedResult = "1__2__3")]
        [TestCase("12_3_", ExpectedResult = "12_3_")]
        [TestCase("12__3__", ExpectedResult = "12__3__")]
        [TestCase("123_qwe_", ExpectedResult = "123_qwe_")]
        [TestCase("123__qwe__", ExpectedResult = "123__qwe__")]
        [TestCase("_123_", ExpectedResult = "<em>123</em>")]
        [TestCase("_123qwe_", ExpectedResult = "<em>123qwe</em>")]
        [TestCase("__123__", ExpectedResult = "<strong>123</strong>")]
        [TestCase("__123qwe__", ExpectedResult = "<strong>123qwe</strong>")]
        public string Should_IgnoreTags_WhichIntersectedWordWithNumber(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("___word___", ExpectedResult = "<strong><em>word</em></strong>")]
        [TestCase("__Here is a _word_ and _another_ :3__", ExpectedResult = "<strong>Here is a <em>word</em> and <em>another</em> :3</strong>")]
        [TestCase("# ___word___", ExpectedResult = "<h1><strong><em>word</em></strong></h1>")]
        [TestCase("# ___[Link](https://google.com)___", ExpectedResult = "<h1><strong><em><a href=https://google.com>Link</a></em></strong></h1>")]
        public string Should_SupportCorrectIntersections(string input)
        {
            return MdRenderer.Render(input);
        }

        [TestCase("[Link](https://google.com/)", ExpectedResult = "<a href=https://google.com/>Link</a>")]
        [TestCase("[Link](https://go_og_le.com/)", ExpectedResult = "<a href=https://go_og_le.com/>Link</a>")]
        [TestCase("[Link](https://go__og__le.com/)", ExpectedResult = "<a href=https://go__og__le.com/>Link</a>")]
        [TestCase("\\[Link](https://w_oo_f.com)", ExpectedResult = "[Link](https://w<em>oo</em>f.com)")]
        [TestCase("___Here is a [Link](https://google.com)___", ExpectedResult = "<strong><em>Here is a <a href=https://google.com>Link</a></em></strong>")]
        public string Should_SupportLinks(string input)
        {
            return MdRenderer.Render(input);
        }

        [Test]
        public void Should_ParseMarkdownSpecCorrectly()
        {
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.FullName;
            var mdSpec = File.ReadAllLines($"{projectDirectory}/MarkdownSpec.md");
            var expectedParseResult = File.ReadAllLines($"{projectDirectory}/MarkdownSpecParsedCorrectly.md");
            for (var i = 0; i < mdSpec.Length; i++)
                MdRenderer.Render(mdSpec[i])
                    .Should().Be(expectedParseResult[i]);
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
                var delta = 2;
                currentTime
                    .Should().BeLessOrEqualTo(expectedTime * delta);
            }
        }
    }
}