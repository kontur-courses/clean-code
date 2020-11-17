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
            
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);
            
            Md.Render(text).Should().Be($"<{tagName}>abc</{tagName}> abc");
        }
        
        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TextAndTag_TextBeforeTag(string mark)
        {
            var text = $"abc {mark}abc{mark}";
            
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);
            
            Md.Render(text).Should().Be($"abc <{tagName}>abc</{tagName}>");
        }
        
        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TagAndText_TextAfterTagWithoutSpace(string mark)
        {
            var text = $"{mark}abc{mark}abc";
            
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);
            
            Md.Render(text).Should().Be($"<{tagName}>abc</{tagName}>abc");
        }
        
        [TestCase("__", TestName = "strong")]
        [TestCase("_", TestName = "em")]
        public void Render_TextAndTag_TextBeforeTagWithoutSpace(string mark)
        {
            var text = $"abc{mark}abc{mark}";
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);
            
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

        [TestCase("__")]
        [TestCase("_")]
        public void Render_PlainText_ScreenedMarksWith3Slashes(string mark)
        {
            var text = $"\\\\\\{mark}abc\\\\\\{mark}";
            
            Md.Render(text).Should().Be($"\\\\{mark}abc\\\\{mark}");
        }

        [Test]
        public void Render_PlainText_SlashInsidePlainText()
        {
            var text = "ab\\c";

            Md.Render(text).Should().Be(text);
        }

        [Test]
        public void Render_PlainText_SlashInsideTag()
        {
            var text = "_ab\\c_";

            Md.Render(text).Should().Be("<em>ab\\c</em>");
        }

        [TestCase("__")]
        [TestCase("_")]
        public void Render_TagInsideWord_MdTagAtBeginningOfWord(string mark)
        {
            var text = $"{mark}ab{mark}c";
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);

            Md.Render(text).Should().Be($"<{tagName}>ab</{tagName}>c");
        }
        
        [TestCase("__")]
        [TestCase("_")]
        public void Render_TagInsideWord_MdTagAtMiddleOfWord(string mark)
        {
            var text = $"a{mark}b{mark}c";
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);

            Md.Render(text).Should().Be($"a<{tagName}>b</{tagName}>c");
        }
        
        [TestCase("__")]
        [TestCase("_")]
        public void Render_TagInsideWord_MdTagAtEndOfWord(string mark)
        {
            var text = $"ab{mark}c{mark}";
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);

            Md.Render(text).Should().Be($"ab<{tagName}>c</{tagName}>");
        }

        [Test]
        public void Render_PlainText_EmptyBetweenMarks()
        {
            const string text = "______";

            Md.Render(text).Should().Be(text);
        }

        [Test]
        public void Render_PlainText_SpaceAfterOpenedMark()
        {
            var text = "_ abc_";

            Md.Render(text).Should().Be(text);
        }
        
        [Test]
        public void Render_PlainText_SpaceBeforeClosedMark()
        {
            var text = "_abc _";

            Md.Render(text).Should().Be(text);
        }

        [Test]
        public void Render_EmTagWithMarkInside_MarkSurroundedBySpacesInsideTag()
        {
            var text = "_abc _ abc_";

            Md.Render(text).Should().Be("<em>abc _ abc</em>");
        }

        [Test]
        public void Render_PlainTextWithMarksInside_MarksInsideDifferentWords()
        {
            var text = "a_bc ab_c";

            Md.Render(text).Should().Be(text);
        }

        [Test]
        public void Render_EmTagInsideStrongTag_EmTagInsideStrongTag()
        {
            var text = "__abc _abc_ abc__";

            Md.Render(text).Should().Be("<strong>abc <em>abc</em> abc</strong>");
        }

        [Test]
        public void Render_EmTagWithDoubleMarkInside_StrongTagInsideEmTag()
        {
            var text = "_abc __abc__ abc_";

            Md.Render(text).Should().Be("<em>abc __abc__ abc</em>");
        }

        [Test]
        public void Render_TagsInsideH1Tag_TagsWorksInsideH1Tag()
        {
            var text = "#abc _def_ __xyz__";

            Md.Render(text).Should().Be("<h1>abc <em>def</em> <strong>xyz</strong></h1>");
        }
    }
}