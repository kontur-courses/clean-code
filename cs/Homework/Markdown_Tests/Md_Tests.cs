using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class Md_Tests
    {

        [TestCase("abc", ExpectedResult = "abc", TestName = "when plain text only")]
        [TestCase("_abc_", ExpectedResult = "<i>abc</i>", TestName = "when italic text only")]
        [TestCase("__abc__", ExpectedResult = "<strong>abc</strong>", TestName = "when strong text only")]
        [TestCase("# abc", ExpectedResult = "<h1>abc</h1>", TestName = "when header only")]
        [TestCase("abc _cde_", ExpectedResult = "abc <i>cde</i>", TestName = "when plain text and italic")]
        public string Render_RenderCorrect(string text)
        {
            return Md.Render(text);
        }

        [Test]
        public void Render_RenderDifferentHeaders()
        {
            var tagTemplate = "######";
            for (var i = 1; i <= 6; i++)
            {
                var tag = tagTemplate[..i];
                var text = $"{tag} abc";
                Md.Render(text).Should().Be($"<h{i}>abc</h{i}>");
            }
        }

        [TestCase("__ab_cd_e__", ExpectedResult = "<strong>ab<i>cd</i>e</strong>")]
        [TestCase("# _abc_", ExpectedResult = "<h1><i>abc</i></h1>")]
        [TestCase("# __ab _cd_ fg__", ExpectedResult = "<h1><strong>ab <i>cd</i> fg</strong></h1>")]
        public string Render_RenderNestedTokens(string text)
        {
            return Md.Render(text);
        }
    }
}