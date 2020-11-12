using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Render_EmTag_OnlyTag()
        {
            var text = "_abc_";
            var result = Md.Render(text);
            result.Should().Be("<em>abc</em>");
        }

        [Test]
        public void Render_PlainText__OnlyText()
        {
            var text = "abc";
            Md.Render(text).Should().Be(text);
        }

        [Test]
        public void Render_StrongTag_OnlyTag()
        {
            var text = "__abc__";
            var result = Md.Render(text);
            result.Should().Be("<strong>abc</strong>");
        }
        
        [Test]
        public void Render_H1Tag_OnlyTag()
        {
            var text = "#abc";
            var result = Md.Render(text);
            result.Should().Be("<h1>abc</h1>");
        }

        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TagAndText_TextAfterTag(string mark)
        {
            var text = $"{mark}abc{mark} abc";
            var tagName = TagBuilder.tags[mark];
            Md.Render(text).Should().Be($"<{tagName}>abc</{tagName}> abc");
        }
        
        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TextAndTag_TextBeforeTag(string mark)
        {
            var text = $"abc {mark}abc{mark}";
            var tagName = TagBuilder.tags[mark];
            Md.Render(text).Should().Be($"abc <{tagName}>abc</{tagName}>");
        }
        
        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TagAndText_TextAfterTagWithoutSpace(string mark)
        {
            var text = $"{mark}abc{mark}abc";
            var tagName = TagBuilder.tags[mark];
            Md.Render(text).Should().Be($"<{tagName}>abc</{tagName}>abc");
        }
        
        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TextAndTag_TextBeforeTagWithoutSpace(string mark)
        {
            var text = $"abc{mark}abc{mark}";
            var tagName = TagBuilder.tags[mark];
            Md.Render(text).Should().Be($"abc<{tagName}>abc</{tagName}>");
        }

        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_PlainText_MarksIntoTextWithDigits(string mark)
        {
            var text = $"abc{mark}12{mark}3";
            Md.Render(text).Should().Be(text);
        }
        
        [TestCase("__")]
        [TestCase("_")]
        public void Render_PlainText_ScreenedMarks(string mark)
        {
            var text = $"\\{mark}abc\\{mark}";
            Md.Render(text).Should().Be($"{mark}abc{mark}");
        }
        
    }
}