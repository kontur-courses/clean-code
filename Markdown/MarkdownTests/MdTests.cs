using System;
using System.Collections.Generic;
using Markdown;
using Markdown.Tag;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MdTests
    {
        private Md md;

        private readonly Dictionary<string, ITag> dictionaryTags = new Dictionary<string, ITag>
        {
            {"_", new SingleUnderLineTag()},
            {"__", new DoubleUnderLineTag()},
            {"#", new SharpTag()}
        };

        [SetUp]
        public void SetUp()
        {
            md = new Md(dictionaryTags);
        }

        [Test]
        public void NullString_ShouldRenderNull()
        {
            Assert.Throws<ArgumentNullException>(() => md.Render(null));
        }

        [Test]
        public void WithoutTags_ShouldRenderWithoutChanges()
        {
            var text = "hello world";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "hello world");
        }


        [Test]
        public void SingleUnderLineTag_ShouldRenderToHtmlTag()
        {
            var text = "hello _world_";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "hello <em>world</em>");
        }

        [Test]
        public void EscapedSingleUnderLineTags_ShouldRenderWithoutHtmlAndEscapedCharacters()
        {
            var text = @"hello \_world\_";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "hello _world_");
        }

        [Test]
        public void DoubleUnderLineTag_ShouldRenderToHtmlTag()
        {
            var text = "hello __world__";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "hello <strong>world</strong>");
        }

        [Test]
        public void SingleUnderLineTagIsInsideDoubleUnderLineTag_ShouldAllTagsRenderToHtmlTag()
        {
            var text = "__hello _happy_ world__";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "<strong>hello <em>happy</em> world</strong>");
        }

        [Test]
        public void DoubleUnderLineTagIsInsideSingleUnderLineTag_ShouldOnlyOutterTagRenderToHtmlTag()
        {
            var text = "_hello __happy__ world_";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "<em>hello __happy__ world</em>");
        }

        [Test]
        public void SingleUnderLineTagInsideTheNumber_ShouldRenderWithoutChanges()
        {
            var text = "_12_3";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "_12_3");
        }

        [Test]
        public void UnPairedTags_ShouldRenderWithoutChanges()
        {
            var text = "__hello _world";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "__hello _world");
        }

        [Test]
        public void IncorrectOpenTag_ShouldRenderWithoutChanges()
        {
            var text = "hello_ world_";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "hello_ world_");
        }

        [Test]
        public void SeveralSingleUnderLineTagsInsideDoubeUnderLineTag_ShouldAllTagsRenderToHtml()
        {
            var text = "__a _b_ _c_ d__";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "<strong>a <em>b</em> <em>c</em> d</strong>");
        }

        [Test]
        public void ManyTags_ShouldAllRenderToHtml()
        {
            var text = "__a__ _b_ #c#";
            var actual = md.Render(text);

            Assert.AreEqual(actual, "<strong>a</strong> <em>b</em> <h1>c</h1>");
        }
    }
}
