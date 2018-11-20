using System;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_RenderToHtml_Should
    {
        private readonly Md md = new Md();
        
        [TestCase("abcd", ExpectedResult = "abcd", TestName = "and contains only letters")]
        [TestCase("as df", ExpectedResult = "as df", TestName = "and contains whitespace")]
        [TestCase("as.df", ExpectedResult = "as.df", TestName = "and contains with non-letter character")]
        public string ReturnTextWithoutChanges_WhenTextDoesNotContainSpecialSymbols(string originalText)
        {
            return md.RenderToHtml(originalText);
        }
        
        [TestCase("___a___", ExpectedResult = "<strong><em>a</em></strong>", TestName = "when em-tag is located immediately after strong-tag")]
        [TestCase("a__b_c_b__a", ExpectedResult = "a<strong>b<em>c</em>b</strong>a", TestName = "when letters between these opening and closing tags")]
        public string BeAbleToRenderEmTagInsideStrongTag(string mdText)
        {
            return md.RenderToHtml(mdText);
        }

        [TestCase("_a__b_", ExpectedResult = "<em>a</em><em>b</em>", TestName = "when tags are em-tags")]
        [TestCase("__a____b__", ExpectedResult = "<strong>a</strong><strong>b</strong>", TestName = "when tags are strong-tags")]
        public string BeAbleToRenderSeveralIdenticalTagsInARow(string mdText)
        {
            return md.RenderToHtml(mdText);
        }

        [TestCase("__a _a", ExpectedResult = "__a _a", TestName = "when tags are unclosed")]
        [TestCase("a_ a__", ExpectedResult = "a_ a__", TestName = "when tags are unopened")]
        public string NotRenderTagSymbolsToHtml(string originalText)
        {
            return md.RenderToHtml(originalText);
        }

        [Test(ExpectedResult = "<em>a _b</em>")]
        public string ReadTextInsideEmTag_UntilMeetingStrictlyClosingTagButNotOpening()
        {
            return md.RenderToHtml("_a _b_");
        }

        [TestCase(@"a \_b_", ExpectedResult = "a _b_", TestName = "Opening tag is screened")]
        [TestCase(@"a _b\_", ExpectedResult = "a _b_", TestName = "Closing tag is screened")]
        public string NotRenderTag_WhenTagSymbolIsScreenedByBackslash(string mdText)
        {
            return md.RenderToHtml(mdText);
        }

        [TestCase("_a_1", ExpectedResult = "_a_1", TestName = "when it is between letter and number")]
        [TestCase("_a1_1", ExpectedResult = "_a1_1", TestName = "when it is between numbers")]
        public string NotRenderClosingTag(string originalText)
        {
            return md.RenderToHtml(originalText);
        }

        [TestCase("a_1_", ExpectedResult = "a_1_", TestName = "when it is between letter and number")]
        [TestCase("a1_1_", ExpectedResult = "a1_1_", TestName = "when it is between numbers")]
        public string NotRenderOpeningTag(string originalText)
        {
            return md.RenderToHtml(originalText);
        }

        [TestCase("a _ a_a_", ExpectedResult = "a _ a<em>a</em>", TestName = "and it can be a opening tag")]
        [TestCase("_a _ a_", ExpectedResult = "<em>a _ a</em>", TestName = "and it can be a closing tag")]
        public string NotRenderTag_WhenItIsSurroundedWithWhitespaces(string mdText)
        {
            return md.RenderToHtml(mdText);
        }

        [TestCase(@"\\_a_", ExpectedResult = @"\<em>a</em>", TestName = "and it's opening tag")]
        [TestCase(@"_a\\_", ExpectedResult = @"<em>a\</em>", TestName = "and it's closing tag")]
        public string BeAbleToRenderTag_WhenBeforeItScreenedBackslash(string mdText)
        {
            return md.RenderToHtml(mdText);
        }

        [Test(ExpectedResult = "<em>a _</em>b<em>_ a</em>")]
        public string RenderEmTagAsPartOfStrongTag_WhenStrongTagInsideEm()
        {
            return md.RenderToHtml("_a __b__ a_");
        }

        [Test(ExpectedResult = "__")]
        public string NotRenderEmptyPairedTag()
        {
            return md.RenderToHtml("__");
        }
    }
}