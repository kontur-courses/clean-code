using FluentAssertions;
using NUnit.Framework;
using Rendering;

namespace MarkdownToHtmlConverterTests
{
    public class MarkdownToHtmlConverterTests
    {
        private IMarkdownConverter converter;

        [SetUp]
        public void Setup()
        {
            converter = Md.ToHtml;
        }

        [TestCaseSource(typeof(GeneralTestCases), nameof(GeneralTestCases.PrimitiveCases))]
        public void BasicTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.BoldTests))]
        public void BoldTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.ItalicTests))]
        public void ItalicTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.BoldItalicInteractionTests))]
        public void BoldItalicInteractionTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.ScreeningTests))]
        public void ScreeningTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        [TestCaseSource(typeof(HeaderTestCases), nameof(HeaderTestCases.HeaderTests))]
        public void HeaderTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        [TestCaseSource(typeof(LinksTestCases), nameof(LinksTestCases.LinkTests))]
        public void LinksTests(string md, string expectedHtml)
        {
            TestConverter(md, expectedHtml);
        }

        private void TestConverter(string md, string expectedHtml)
        {
            TestContext.Progress.WriteLine(md);
            converter.Convert(md)
                .Should()
                .Be(expectedHtml);
            TestContext.Progress.WriteLine(expectedHtml);
        }
    }
}