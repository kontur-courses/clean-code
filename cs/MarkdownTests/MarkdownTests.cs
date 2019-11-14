using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTests
    {
        private Md md;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            md = new Md();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Render_DoesNotThrow_OnNullOrWhitespace(string mdText)
        {
            Assert.DoesNotThrow(() => md.Render(mdText));
        }

        [TestCase(@"\_text\_", ExpectedResult = @"_text_")]
        [TestCase(@"\__text\__", ExpectedResult = @"_<em>text_</em>")]
        [TestCase(@"\text text", ExpectedResult = @"\text text")]
        [TestCase(@"text \ text", ExpectedResult = @"text \ text")]
        [TestCase(@"text text \", ExpectedResult = @"text text \")]
        public string Render_ShouldCorrectRenderDisablingSymbol(string mdText)
        {
            return md.Render(mdText);
        }

        [TestCase(Style.Italic, "text _italic", 5, ExpectedResult = true)]
        [TestCase(Style.Bold, "text __bold", 5, ExpectedResult = true)]
        [TestCase(Style.Italic, "text ", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "text %", 5, ExpectedResult = false)]
        public bool Style_IsTag_ReturnsCorrectValue(Style style, string text, int index)
        {
            return style.IsTag(ref text, index);
        }

        [TestCase(Style.Italic, "text_", 4, ExpectedResult = true)]
        [TestCase(Style.Italic, "text _", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "t00t_", 4, ExpectedResult = false)]
        public bool Style_CanEnd_ReturnsCorrectValue(Style style, string text, int index)
        {
            return style.CanEnd(ref text, index);
        }

        [TestCase(Style.Italic, "text _italic_", 5, ExpectedResult = true)]
        [TestCase(Style.Bold, "text __bold__", 5, ExpectedResult = true)]
        [TestCase(Style.Italic, "text ", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "text _", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "_t00t_", 0, ExpectedResult = false)]
        public bool Style_CanBegin_ReturnsCorrectValue(Style style, string text, int index)
        {
            return style.CanBegin(ref text, index, new Stack<(Style style, int endIndex)>(), out int _);
        }

        [TestCase("__bold _bolditalic_ bold__", ExpectedResult = "<strong>bold <em>bolditalic</em> bold</strong>")]
        [TestCase("_italic __nonbolditalic__ italic_", ExpectedResult = "<em>italic _</em>nonbolditalic<em>_ italic</em>")]
        public string Render_ShouldCorrectRender_WhenOneStyleIsIntoAnother(string mdText)
        {
            return md.Render(mdText);
        }

        [TestCase("_italicBegin _italicNotBegin italicEnd_ italicNotEnd_", ExpectedResult = "<em>italicBegin _italicNotBegin italicEnd</em> italicNotEnd_")]
        [TestCase("__boldBegin __boldNotBegin boldEnd__ boldNotEnd__", ExpectedResult = "<strong>boldBegin __boldNotBegin boldEnd</strong> boldNotEnd__")]
        public string Render_ShouldCorrectRender_WhenOneStyleIsIntoSameStyle(string mdText)
        {
            return md.Render(mdText);
        }

        [TestCase("__boldBegin _italicBegin boldEnd__ italicEnd_", ExpectedResult = "<strong>boldBegin _italicBegin boldEnd</strong> italicEnd_")]
        public string Render_ShouldCorrectRender_WhenStylesBoundsAreIntersected(string mdText)
        {
            return md.Render(mdText);
        }

        [Test]
        public void Render_Duration_ShouldBeInLinearDependencyOfTextLength()
        {
            var testText = @"_Text_ _piece_ _for_ __Markdown__ _class_ _perfomance_ _test_. _Word_with_numbers_123_. \_Backslashed symbols\_. ";

            var factors = new List<double>();
            long previousDuration = 0;
            for (int i = 1; i <= 10; i++)
            {
                testText += testText; //test text length = 2^i
                var sw = Stopwatch.StartNew();
                md.Render(testText);
                sw.Stop();
                var factor = i > 1 ? (double)sw.ElapsedTicks / previousDuration : 0;
                previousDuration = sw.ElapsedTicks;
                factors.Add(factor);
            }
            factors.ForEach(durationFactor => durationFactor.Should().BeLessOrEqualTo(2.5, $"text length has been increased by 2"));
        }
    }
}
