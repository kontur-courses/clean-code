using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void SupportCursiveTag()
        {
            md.Render("I'm a _cursive_ :3")
                .Should().Be("I'm a <em>cursive</em> :3");
        }

        [Test]
        public void SupportBoldTag()
        {
            md.Render("I'm a __bold__ :3")
                .Should().Be("I'm a <strong>bold</strong> :3");
        }

        [Test]
        public void SupportHeaderTag()
        {
            md.Render("# Header")
                .Should().Be("<h1>Header</h1>");
        }

        [Test]
        public void SupportShielding()
        {
            md.Render("\\# Cat\\s \\\\__are__ \\_cute_")
                .Should().Be("# Cat\\s \\<strong>are</strong> _cute_");
        }

        [Test]
        public void IgnoreTag_WhenWhiteSpace_AfterOpener()
        {
            md.Render("# Ha-ha _ we're_ __ ignored__")
                .Should().Be("<h1>Ha-ha _ we're_ __ ignored__</h1>");
        }

        [Test]
        public void IgnoreTag_WhenWhiteSpace_BeforeCloser()
        {
            md.Render("# Ha _gotcha _ :3_ __And __ u2__")
                .Should().Be("<h1>Ha <em>gotcha _ :3</em> <strong>And __ u2</strong></h1>");
        }

        [Test]
        public void IgnoreEmptyTags()
        {
            md.Render("____ _He is a_ __traitor!__")
                .Should().Be("____ <em>He is a</em> <strong>traitor!</strong>");
        }

        [Test]
        public void IgnoreDifferentTagIntersections()
        {
            md.Render("_Uhm... what __the _f**k__ __is_ ___going_ on__ here?")
                .Should().Be("_Uhm... what __the _f**k__ <strong>is_ __<em>going</em> on</strong> here?");
        }

        [Test]
        public void IgnoreBoldTag_WhenInsideOfCursiveTag()
        {
            md.Render("_Ugh :( __I'm ignored again__ :(_")
                .Should().Be("<em>Ugh :( __I'm ignored again__ :(</em>");
        }

        [Test]
        public void IgnoreTags_WhichCovering_DifferentWordsPartially()
        {
            md.Render("# I'm a pa_rt an_d I'm ignored :(")
                .Should().Be("<h1>I'm a pa_rt an_d I'm ignored :(</h1>");
        }

        [Test]
        public void IgnoreTags_WhereIntersectedNumber()
        {
            md.Render("_12_3 3_2_1 12_3_ _123_ qwe_123_")
                .Should().Be("_12_3 3_2_1 12_3_ <em>123</em> qwe_123_");
        }

        [Test]
        public void BeLinearEfficient()
        {
            var sw = new Stopwatch();
            var mdString = "_Tag_ __Bold Tag__ /_not tag_ ";
            var sb = new StringBuilder(mdString);

            md.Render("_warm up_");

            sw.Start();
            md.Render(mdString);
            sw.Stop();
            var previousTime = sw.ElapsedTicks;

            for (var i = 1; i < 500; i++)
            {
                sb.Append(mdString);
                var markdown = sb.ToString();
                sw.Restart();
                md.Render(markdown);
                sw.Stop();
                var currentTime = sw.ElapsedTicks;
                currentTime
                    .Should().BeLessThan((long)(previousTime * Math.Log(previousTime)));
                previousTime = currentTime;
            }
        }
    }
}