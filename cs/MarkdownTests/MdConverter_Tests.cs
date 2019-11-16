using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdConverter_Tests
    {
        [TestCase("just text", 1)]
        [TestCase("text _emTag_", 2)]
        [TestCase("text _cutEmTag", 1)]
        [TestCase("text __strongTag__ _cutEmTag", 2)]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", 1)]
        public void MdConverter_HasPairTags_RightDefinesPairTags(string text, int expectedPairTokens)
        {
            var tokens = new MdParser(text).GetTokens();
            var converter = new MdConverter(tokens, text);
            var pairTagsCount = tokens.Count(token => converter.HasPairTags(token));
            pairTagsCount.Should().Be(pairTagsCount);
        }
        
        [TestCase("just text", "just text")]
        [TestCase("text _emTag_", "text ", "<em>emTag</em>")]
        [TestCase("text _cutEmTag", "text ", "_cutEmTag")]
        [TestCase("text __strongTag__ _cutEmTag", "text ", "<strong>strongTag</strong>", " ", "_cutEmTag")]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", "<em>emStart __strongTagInEmTag__ emFinish</em>")]
        public void MdConverter_ConvertToHtml_RightResult(string text, params string[] expectedHtmlEquivalents)
        {
            var tokens = new MdParser(text).GetTokens();
            var converter = new MdConverter(tokens, text);
            var htmlEquivalents = tokens.Select(token => converter.ConvertToHtml(token)).ToArray();
            htmlEquivalents.Should().BeEquivalentTo(expectedHtmlEquivalents);
        }
    }
}