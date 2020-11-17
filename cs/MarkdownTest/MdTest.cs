using System.IO;
using Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MdTest_Should
    {
        [TestCase("")]
        [TestCase(null)]
        public void RenderShouldReturnEmpty_WhenInputEmptyOrNull(string input)
        {
            Md.Render(input).Should().BeEmpty();
        }

        [Test]
        public void RenderShouldReturnSame_WhenInputWithoutTags()
        {
            var input = "Input string without tags";
            Md.Render(input).Should().Be(input);
        }

        [TestCase("_Input string with em_", "<em>Input string with em</em>")]
        [TestCase("__Input string with strong__", "<strong>Input string with strong</strong>")]
        [TestCase("# Header", "<h1>Header</h1>")]
        public void RenderShouldReturnCorrectTag(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("## Test", "## Test")]
        [TestCase("# # Test", "<h1># Test</h1>")]
        [TestCase("# Test # Test", "<h1>Test # Test</h1>")]
        public void RenderShouldReturnCorrect_WhenMultipleHashSymbols(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("_Not closed italic", "_Not closed italic")]
        [TestCase("__Not closed bold", "__Not closed bold")]
        [TestCase("__word __", "__word __")]
        [TestCase("_ word_", "_ word_")]
        [TestCase("__Word w__ord", "__Word w__ord")]
        [TestCase("Notcl__osedbold", "Notcl__osedbold")]
        [TestCase("Notclo_seditalic", "Notclo_seditalic")]
        public void RenderShouldntConvert_WhenTagNotClosed(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("_Bold can't be __inside__ italic_ tag", "<em>Bold can't be __inside__ italic</em> tag")]
        [TestCase("l_on__gw__or_d", "l<em>on__gw__or</em>d")]
        public void RenderShouldntConvertBold_WhenBoldInsideItalic(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("__Italic _inside_ bold__", "<strong>Italic <em>inside</em> bold</strong>")]
        [TestCase("__Italic_inside_bold__", "<strong>Italic<em>inside</em>bold</strong>")]

        public void RenderShouldConvert_WhenItalicInsideBold(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase(@"__\_\___", "<strong>__</strong>")]
        [TestCase(@"\_Input string\_", "_Input string_")]
        [TestCase(@"\_Input _string_", "_Input <em>string</em>")]
        [TestCase(@"\\__Input string__", "\\<strong>Input string</strong>")]
        [TestCase(@"\# Input string", "# Input string")]
        public void Render_BackslashesShouldEscaping(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderRenderShouldntConvert_WhenEmptyTag()
        {
            var input = "____";
            var output = "____";
            Md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderShouldReturnCorrect_ExampleFromSpecification()
        {
            var input = "# Заголовок __с _разными_ символами__";
            var output = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";
            Md.Render(input).Should().Be(output);
        }

        [TestCase("__когда _два тега__ пересекаются_", "__когда _два тега__ пересекаются_")]
        [TestCase("_когда __два тега_ пересекаются__", "_когда __два тега_ пересекаются__")]
        [TestCase("_когда__дватега_пересекаются__", "_когда__дватега_пересекаются__")]
        public void RenderShouldntConvert_WhenTagsIntersect(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("# # Заголовок", "<h1># Заголовок</h1>")]
        [TestCase("##Заголовок", "##Заголовок")]
        public void RenderShouldParseCorrect_WhenMultipleHashSymbols(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("___Input string___", "_<strong>Input string</strong>_")]
        [TestCase("Inp___utst___ring", "Inp_<strong>utst</strong>_ring")]
        public void RenderShouldParseCorrect_WhenTagContainsMoreThanTwoUnderscores(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("_wo_rd", "<em>wo</em>rd")]
        [TestCase("w_or_d", "w<em>or</em>d")]
        [TestCase("wo_rd_", "wo<em>rd</em>")]
        [TestCase("__wo__rd", "<strong>wo</strong>rd")]
        [TestCase("w__or__d", "w<strong>or</strong>d")]
        [TestCase("wo__rd__", "wo<strong>rd</strong>")]
        public void RenderShouldParse_WhenSingleTagInWord(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("цифрами_12_3", "цифрами_12_3")]
        [TestCase("1word_with__digit_", "1word_with__digit_")]
        [TestCase("word_with__digit_1", "word_with__digit_1")]
        public void RenderShouldntFormat_WhenDigitsInWord(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase("[link](source)", "<a href=\"source\">link</a>")]
        [TestCase("[link](source \"alt text\")", "<a href=\"source\" alt=\"alt text\">link</a>")]
        [TestCase("[linked text](google.com \"Google.inc\")",
            "<a href=\"google.com\" alt=\"Google.inc\">linked text</a>")]
        public void RenderShouldReturnCorrect_WhenLinkTag(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }
        
        [TestCase("# Header [link](source)", "<h1>Header <a href=\"source\">link</a></h1>")]
        [TestCase("__[link](source)__", "<strong><a href=\"source\">link</a></strong>")]
        [TestCase("_[link](source)_", "<em><a href=\"source\">link</a></em>")]
        [TestCase("# _[link](source)_", "<h1><em><a href=\"source\">link</a></em></h1>")]
        public void RenderShouldReturnCorrect_WhenLinkInTag(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }

        [TestCase(@"\[[link\]](source)", "[<a href=\"source\">link]</a>")]
        [TestCase(@"[link](source\))", "<a href=\"source)\">link</a>")]
        public void RenderShouldEscape_WhenLinkTagEscaped(string input, string output)
        {
            Md.Render(input).Should().Be(output);
        }
        
        [Test]
        public void RenderShouldReturnCorrect_WhenAllTagIncluded()
        {
            var input = "# Заголовок __с _разными_ [link](source) символами__";
            var output = "<h1>Заголовок <strong>с <em>разными</em> <a href=\"source\">link</a> символами</strong></h1>";
            Md.Render(input).Should().BeEquivalentTo(output);
        }
        
        [Test]
        public void RenderShouldParseSpecification()
        {
            var testDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent;
            if (testDirectory != null)
            {
                var mdText = new StreamReader($"{testDirectory.FullName}\\MarkdownSpec.md").ReadToEnd();
                var htmlText = new StreamReader($"{testDirectory.FullName}\\MarkdownSpec.html").ReadToEnd();
                Md.Render(mdText).Should().Be(htmlText);
            }
        }
    }
}