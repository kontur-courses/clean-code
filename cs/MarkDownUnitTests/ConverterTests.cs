using FluentAssertions;
using Markdown.Converter;
using Markdown.Tags;
using NUnit.Framework;

namespace MarkDownUnitTests
{
    [TestFixture]
    public class ConverterTests
    {
        private Converter converter;


        [SetUp]
        public void SetUp()
        {
            converter = new Converter(new MdTagStorage(), new HtmlTagStorage());
        }


        [TestCase("Text without tags", "Text without tags", TestName = "Text without tags")]
        [TestCase("Some _italic_ text", "Some <em>italic</em> text", TestName = "Text with italic tags")]
        [TestCase("Some __bold__ text", "Some <strong>bold</strong> text", TestName = "Text with bold tags")]
        [TestCase("# Simple header\n", "<h1>Simple header</h1>", TestName = "Text with header tags")]
        [TestCase("Header tag# inside text\n", "Header tag# inside text\n",
            TestName = "Header tag inside text doesn't work")] 
        [TestCase("_italic_ tag and __bold__ tags", "<em>italic</em> tag and <strong>bold</strong> tags",
            TestName = "Text with two not intersecting tags")]
        [TestCase("Italic _unpaired tags", "Italic _unpaired tags", TestName = "Unpaired italic tag doesn't work")]
        [TestCase("Bold __unpaired tags", "Bold __unpaired tags", TestName = "Unpaired bold tag doesn't work")]
        [TestCase("# Header unpaired tags", "<h1>Header unpaired tags</h1>",
            TestName = "Unpaired opening header tag works")]
        [TestCase("Unpaired _italic and __bold tags", "Unpaired _italic and __bold tags",
            TestName = "Two unpaired tags don't work")]
        [TestCase(@"Escaped \_italic\_ tags", "Escaped _italic_ tags", TestName = "Escaped tag doesn't work")]
        [TestCase(@"Double \\escaped character", @"Double \escaped character",
            TestName = "Double escape-character escape himself")]
        [TestCase(@"Escape character insi\de word", @"Escape character insi\de word",
            TestName = "Escape character inside word doesn't work")]
        [TestCase("__bold _italic_ bold__", "<strong>bold <em>italic</em> bold</strong>",
            TestName = "Italic tags between bold tags work")]
        [TestCase("_italic __bold__ italic_", "<em>italic __bold__ italic</em>",
            TestName = "Bold tags between italic tags don't work")]
        [TestCase("_italic __bold italic_ bold__", "_italic __bold italic_ bold__",
            TestName = "Intersecting bold and italic tags don't work")]
        [TestCase("_Ita_lic tags", "<em>Ita</em>lic tags", TestName = "Italic tags work in the beginning of word")]
        [TestCase("I_ta_lic tags", "I<em>ta</em>lic tags", TestName = "Italic tags work in the middle of word")]
        [TestCase("Ita_lic_ tags", "Ita<em>lic</em> tags", TestName = "Italic tags work in the end of word")]
        [TestCase("Ita_lic tag_s", "Ita_lic tag_s", TestName = "Italic tags in different words don't work")]
        [TestCase("Space after opening_ italic_ tag", "Space after opening_ italic_ tag",
            TestName = "Italic opening tag doesn't work when space after")]
        [TestCase("Space before closing _italic _ tag", "Space before closing _italic _ tag",
            TestName = "Italic closing tag doesn't work when space before")]
        [TestCase("Italic tag inside digits _123_45", "Italic tag inside digits _123_45",
            TestName = "Italic tags inside digits don't work")]
        [TestCase("__Bol__d tags", "<strong>Bol</strong>d tags", TestName = "Bold tags work in the beginning of word")]
        [TestCase("B__ol__d tags", "B<strong>ol</strong>d tags", TestName = "Bold tags work in the middle of word")]
        [TestCase("Bol__d__ tags", "Bol<strong>d</strong> tags", TestName = "Bold tags work in the end of word")]
        [TestCase("Bo__ld tag__s", "Bo__ld tag__s", TestName = "Bold tags in different words don't work")]
        [TestCase("Space after opening__ bold__ tag", "Space after opening__ bold__ tag",
            TestName = "Bold opening tag doesn't work when space after")]
        [TestCase("Space before closing __bold __ tag", "Space before closing __bold __ tag",
            TestName = "Bold closing tag doesn't work when space before")]
        [TestCase("Bold tag inside digits _123_45", "Bold tag inside digits _123_45",
            TestName = "Bold tags inside digits don't work")]
        [TestCase("Bold tags around ____ empty string", "Bold tags around ____ empty string",
            TestName = "Tags around empty string don't work")]
        [TestCase("# Header with __bold and _italic_ tags__\n",
            "<h1>Header with <strong>bold and <em>italic</em> tags</strong></h1>",
            TestName = "Text with headers and two not intersecting tags")]
        public void Convert_ReturnsConvertedToHtmlText_WhenTextWithMdTags(string inputText, string convertedText)
        {
            var actualConvertedText = converter.Convert(inputText);

            actualConvertedText.Should().BeEquivalentTo(convertedText);
        }
    }
}