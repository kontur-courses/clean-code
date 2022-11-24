using System.Runtime.InteropServices;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    //TODO refactoring test: delete SimpleText and divide tests into subgroups
    public class MarkdownRender_Tests
    {
        [Test]
        public void SimpleText()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. abdc dsa Morbi nec.";

            var html = MarkdownRender.Render(new HtmlRender(), text);

            html.Should().Be(text);
        }


        [TestCase("__openBold word _openItalic word closeItalic_ word closeBold__",
            "<strong>openBold word <em>openItalic word closeItalic</em> word closeBold</strong>")]
        [TestCase("_a", "_a")]
        [TestCase("_a_", "<em>a</em>")]
        [TestCase("__a__", "<strong>a</strong>")]
        [TestCase("__BoldItalic_Italic_ItalicBold__", "<strong>BoldItalic<em>Italic</em>ItalicBold</strong>")]
        [TestCase("a_a a_a", "a_a a_a")]
        [TestCase("__asda_dasda__", "__asda_dasda__")]
        [TestCase("____", "____")]
        [TestCase("__", "__")]
        [TestCase("___a___", "<strong><em>a</em></strong>")]
        [TestCase(@"\_a", "_a")]
        [TestCase(@"\_a_", "_a_")]
        [TestCase(@"\\_a_", "\\<em>a</em>")]
        [TestCase(@"_\a_", @"<em>\a</em>")]
        [TestCase(@"_a\_", "_a_")]
        [TestCase(@"a__", "a__")]
        [TestCase(@"\__a_", "_<em>a</em>")]
        [TestCase("_двойных__", "_двойных__")]
        [TestCase("__пересечени€ _двойных__ и одинарных_", "__пересечени€ _двойных__ и одинарных_")]
        [TestCase("_ab__cd_", "<em>ab__cd</em>")]
        [TestCase("__a _b_ _c_ d__", "<strong>a <em>b</em> <em>c</em> d</strong>")]
        [TestCase("_a __c__ d_", "<em>a __c__ d</em>")]
        [TestCase("# aba", "<h1> aba</h1>")]
        [TestCase("#aba", "#aba")]
        [TestCase("_ab  #aba ba_", "<em>ab #aba ba</em>")]
        [TestCase("_ab  \\#aba ba_", "<em>ab #aba ba</em>")]
        [TestCase("__ab  \\#aba ba__", "<strong>ab #aba ba</strong>")]
        [TestCase("# __a _b_ _c_ d__", "<h1> <strong>a <em>b</em> <em>c</em> d</strong></h1>")]
        [TestCase("# «аголовок __с _разными_ символами__",
            "<h1> «аголовок <strong>с <em>разными</em> символами</strong></h1>")]
        [TestCase("ab #", "ab #")]
        [TestCase("ab # ab", "ab # ab")]
        [TestCase("# ", "<h1></h1>")] // ориентировалс€ на поведение обычного маркдауна
        [TestCase("#", "<h1></h1>")] // ориентировалс€ на поведение обычного маркдауна
        [TestCase("a #", "a #")] // ориентировалс€ на поведение обычного маркдауна
        public void MarkdownRenderShould_Correspond(string text, string expected)
        {
            var html = MarkdownRender.Render(new HtmlRender(), text);

            html.Should().Be(expected);
        }
    }
}