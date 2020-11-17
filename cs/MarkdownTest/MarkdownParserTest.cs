using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MarkdownParserTest_Should
    {
        private MarkdownParser parser;
        [SetUp]
        public void SetUp()
        {
           parser = new MarkdownParser();
        }

        [Test]
        public void RenderShouldReturnSame_WhenInputWithoutTags()
        {
            var input = "Input string without tags";
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(Tag.NoFormatting);
        }

        [TestCase("_Input string with em_", Tag.Italic, Tag.NoFormatting)]
        [TestCase("__Input string with strong__", Tag.Bold, Tag.NoFormatting)]
        [TestCase("# Header", Tag.Heading, Tag.NoFormatting)]
        public void RenderShouldReturnCorrectTag(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("## Test", Tag.NoFormatting)]
        [TestCase("# # Test", Tag.Heading, Tag.NoFormatting)]
        [TestCase("# Test # Test", Tag.Heading, Tag.NoFormatting)]
        public void RenderShouldReturnCorrect_WhenMultipleHashSymbols(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("_Not closed italic", Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("__Not closed bold", Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("__word __", Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("_ word_", Tag.NoFormatting)]
        [TestCase("__Word w__ord", Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("Notcl__osedbold", Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("Notclo_seditalic", Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting)]
        public void RenderShouldntConvert_WhenTagNotClosed(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("_Bold can't be __inside__ italic_ tag")]
        [TestCase("_on__gw__or_d")]
        public void RenderShouldntConvertBold_WhenBoldInsideItalic(string input)
        {
            var output = new[]
            {
                Tag.Italic, Tag.NoFormatting, Tag.NoFormatting,
                Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting
            };
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("__Italic _inside_ bold__")]
        [TestCase("__Italic_inside_bold__")]
        public void RenderShouldConvert_WhenItalicInsideBold(string input)
        {
            var output = new[]
            {
                Tag.Bold, Tag.Italic, Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting
            };
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase(@"__\_\___", Tag.Bold, Tag.NoFormatting)]
        [TestCase(@"\_Input string\_", Tag.NoFormatting)]
        [TestCase(@"\_Input _string_", Tag.NoFormatting, Tag.Italic, Tag.NoFormatting)]
        [TestCase(@"\\__Input string__", Tag.NoFormatting, Tag.Bold, Tag.NoFormatting)]
        [TestCase(@"\# Input string", Tag.NoFormatting)]
        public void Render_BackslashesShouldEscaping(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [Test]
        public void RenderRenderShouldntConvert_WhenEmptyTag()
        {
            var input = "____";
            var output = new []{Tag.NoFormatting};
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("__когда _два тега__ пересекаются_")]
        [TestCase("_когда __два тега_ пересекаются__")]
        [TestCase("_когда__дватега_пересекаются__")]
        public void RenderShouldntConvert_WhenTagsIntersect(string input)
        {
            var output = new[] {Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting};
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("# # Заголовок", Tag.Heading, Tag.NoFormatting)]
        [TestCase("##Заголовок", Tag.NoFormatting)]
        public void RenderShouldParseCorrect_WhenMultipleHashSymbols(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("___Input string___")]
        [TestCase("Inp___utst___ring")]
        public void RenderShouldParseCorrect_WhenTagContainsMoreThanTwoUnderscores(string input)
        {
            var output = new[] {Tag.NoFormatting, Tag.Bold, Tag.NoFormatting, Tag.NoFormatting};
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("_wo_rd", Tag.Italic, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("w_or_d", Tag.NoFormatting, Tag.Italic, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("wo_rd_", Tag.NoFormatting, Tag.Italic, Tag.NoFormatting)]
        [TestCase("__wo__rd", Tag.Bold, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("w__or__d", Tag.NoFormatting, Tag.Bold, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("wo__rd__", Tag.NoFormatting, Tag.Bold, Tag.NoFormatting)]
        public void RenderShouldParse_WhenSingleTagInWord(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("цифрами_12_3", Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting)]
        [TestCase("1word_with__digit_", Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting, Tag.NoFormatting)]
        public void RenderShouldntFormat_WhenDigitsInWord(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("[link](source)")]
        [TestCase("[link](source \"alt text\")")]
        [TestCase("[linked text](google.com \"Google.inc\")")]
        public void RenderShouldReturnCorrect_WhenLinkTag(string input)
        {
            var output = new[] {Tag.Link, Tag.NoFormatting};
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase("# Header [link](source)", Tag.Heading, Tag.NoFormatting, Tag.Link, Tag.NoFormatting)]
        [TestCase("__[link](source)__", Tag.Bold, Tag.Link, Tag.NoFormatting)]
        [TestCase("_[link](source)_", Tag.Italic, Tag.Link, Tag.NoFormatting)]
        [TestCase("# _[link](source)_", Tag.Heading, Tag.Italic, Tag.Link, Tag.NoFormatting)]
        public void RenderShouldReturnCorrect_WhenLinkInTag(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        [TestCase(@"\[[link\]](source)", Tag.NoFormatting, Tag.Link, Tag.NoFormatting)]
        [TestCase(@"[link](source\))", Tag.Link, Tag.NoFormatting)]
        public void RenderShouldEscape_WhenLinkTagEscaped(string input, params Tag[] output)
        {
            GetTagsSequence(parser.Parse(input)).Should().BeEquivalentTo(output);
        }

        private IEnumerable<Tag> GetTagsSequence(TagInfo root)
        {
            var sequence = new List<Tag>();
            foreach (var elem in root.Content)
            {
                sequence.Add(elem.Tag);
                sequence.AddRange(GetTagsSequence(elem));
            }

            return sequence;
        }
    }
}