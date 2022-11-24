using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class InteractionShould
    {
        [Test]
        public void RenderMarkdownString_WhenItalicInsideStrong()
        {
            var markdownString = "__e some _text_ e__";
            var result = Md.Render(markdownString);
            result.Should().Be("\\<strong>e some \\<em>text\\</em> e\\</strong>");
        }

        [Test]
        public void NotRenderStrongTags_WhenStrongInsideItalic()
        {
            var markdownString = "_e some __text__ e_";
            var result = Md.Render(markdownString);
            result.Should().Be("\\<em>e some __text__ e\\</em>");
        }

        [Test]
        public void NotRenderMarkdownString_WhenNumbersAreTagged()
        {
            var markdownString = "_12_re __12__vo lu1_23_4ti on1__23__4a r12_34_ y12__34__";
            var result = Md.Render(markdownString);
            result.Should().Be("_12_re __12__vo lu1_23_4ti on1__23__4a r12_34_ y12__34__");
        }

        [Test]
        public void RenderMarkdownString_WhenTagsInOneWord()
        {
            var markdownString = "_so_me c_rin_ge te_xt_ __fo__r b__r__o ro__fl__";
            var result = Md.Render(markdownString);
            result.Should().Be("\\<em>so\\</em>me c\\<em>rin\\</em>ge te\\<em>xt\\</em> \\<strong>fo\\</strong>r b\\<strong>r\\</strong>o ro\\<strong>fl\\</strong>");
        }

        [Test]
        public void NotRenderMarkdownString_WhenTagsInDifferentWords()
        {
            var markdownString = "so_me te_xt f__or rof__l";
            var result = Md.Render(markdownString);
            result.Should().Be("so_me te_xt f__or rof__l");
        }

        [Test]
        public void NotRenderMarkdownString_WhenTagsNotPaired()
        {
            var markdownString = "_some text__";
            var result = Md.Render(markdownString);
            result.Should().Be("_some text__");
        }

        [Test]
        public void NotRenderMarkdownString_WhenSpaceIsAfterBeginningTag()
        {
            var markdownString = "some_ cringe_ text__ bro__";
            var result = Md.Render(markdownString);
            result.Should().Be("some_ cringe_ text__ bro__");
        }

        [Test]
        public void NotRenderMarkdownString_WhenSpaceIsBeforeEndingTag()
        {
            var markdownString = "_some _cringe __text __bro";
            var result = Md.Render(markdownString);
            result.Should().Be("_some _cringe __text __bro");
        }

        [Test]
        public void NotRenderMarkdownString_WhenTagsAreCrossed()
        {
            var markdownString = "_some __cringe_ text__";
            var result = Md.Render(markdownString);
            result.Should().Be("_some __cringe_ text__");
        }

        [Test]
        public void NotRenderMarkdownString_WhenOnlyTags()
        {
            var markdownString = " __  ____";
            var result = Md.Render(markdownString);
            result.Should().Be(" __  ____");
        }
    }
}
