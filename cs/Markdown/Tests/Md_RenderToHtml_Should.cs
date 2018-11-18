using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_RenderToHtml_Should
    {
        private readonly Md md = new Md();
        
        [TestCase("abcd", TestName = "and contains only letters")]
        [TestCase("as df", TestName = "and contains whitespace")]
        [TestCase("as.df", TestName = "and contains with non-letter character")]
        public void ReturnTextWithoutChanges_WhenTextDoesNotContainSpecialSymbols(string originalText)
        {
            CheckRenderingFromMdToHtml(mdText: originalText, expHtmlText: originalText);
        }
        
        [TestCase("___a___", "<strong><em>a</em></strong>", TestName = "when em-tag is located immediately after strong-tag")]
        [TestCase("a__b_c_b__a", "a<strong>b<em>c</em>b</strong>a", TestName = "when letters between these opening and closing tags")]
        public void BeAbleToRenderEmTagInsideStrongTag(string mdText, string expHtmlText)
        {
            CheckRenderingFromMdToHtml(mdText, expHtmlText);
        }

        [TestCase("_a__b_", "<em>a</em><em>b</em>", TestName = "when tags are em-tags")]
        [TestCase("__a____b__", "<strong>a</strong><strong>b</strong>", TestName = "when tags are strong-tags")]
        public void BeAbleToRenderSeveralIdenticalTagsInARow(string mdText, string expHtmlText)
        {
            CheckRenderingFromMdToHtml(mdText, expHtmlText);
        }

        [TestCase("__a _a", TestName = "when tags are unclosed")]
        [TestCase("a_ a__", TestName = "when tags are unopened")]
        public void NotRenderTagSymbolsToHtml(string originalText)
        {
            CheckRenderingFromMdToHtml(mdText: originalText, expHtmlText: originalText);
        }

        [Test]
        public void ReadTextInsideEmTag_UntilMeetingStrictlyClosingTagButNotOpening()
        {
            CheckRenderingFromMdToHtml(mdText: "_a _b_", expHtmlText: "<em>a _b</em>");
        }

        [TestCase(@"a \_b_", "a _b_", TestName = "Opening tag is screened")]
        [TestCase(@"a _b\_", "a _b_", TestName = "Closing tag is screened")]
        public void NotRenderTag_WhenTagSymbolIsScreenedByBackslash(string mdText, string expHtmlText)
        {
            CheckRenderingFromMdToHtml(mdText, expHtmlText);
        }

        [TestCase("_a_1", TestName = "when it is between letter and number")]
        [TestCase("_a1_1", TestName = "when it is between numbers")]
        public void NotRenderClosingTag(string originalText)
        {
            CheckRenderingFromMdToHtml(mdText: originalText, expHtmlText: originalText);
        }

        [TestCase("a_1_", TestName = "when it is between letter and number")]
        [TestCase("a1_1_", TestName = "when it is between numbers")]
        public void NotRenderOpeningTag(string originalText)
        {
            CheckRenderingFromMdToHtml(mdText: originalText, expHtmlText: originalText);
        }

        [TestCase("a _ a_a_", "a _ a<em>a</em>", TestName = "and it can be a opening tag")]
        [TestCase("_a _ a_", "<em>a _ a</em>", TestName = "and it can be a closing tag")]
        public void NotRenderTag_WhenItIsSurroundedWithWhitespaces(string mdText, string expHtmlText)
        {
            CheckRenderingFromMdToHtml(mdText, expHtmlText);
        }

        [TestCase(@"\\_a_", @"\<em>a</em>", TestName = "and it's opening tag")]
        [TestCase(@"_a\\_", @"<em>a\</em>", TestName = "and it's closing tag")]
        public void BeAbleToRenderTag_WhenBeforeItScreenedBackslash(string mdText, string expHtmlText)
        {
            CheckRenderingFromMdToHtml(mdText, expHtmlText);
        }

        [Test]
        public void RenderEmTagAsPartOfStrongTag_WhenStrongTagInsideEm()
        {
            CheckRenderingFromMdToHtml(mdText: "_a __b__ a_", expHtmlText: "<em>a _</em>b<em>_ a</em>");
        }

        [Test]
        public void NotRenderEmptyPairedTag()
        {
            CheckRenderingFromMdToHtml(mdText: "__", expHtmlText: "__");
        }

        private void CheckRenderingFromMdToHtml(string mdText, string expHtmlText)
        {
            var res = md.RenderToHtml(mdText);
            res.Should().Be(expHtmlText);
        }
    }
}