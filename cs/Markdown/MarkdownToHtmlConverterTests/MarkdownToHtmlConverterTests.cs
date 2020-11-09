using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using HtmlMarkdownRenderer;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public class MarkdownToHtmlConverterTests
    {
        private HtmlMarkdownConverter converter;

        private static readonly DirectoryInfo testCasesFolder =
            new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestCases"));

        [SetUp]
        public void Setup()
        {
            converter = new HtmlMarkdownConverter();
        }

        [TestCaseSource(nameof(FilesTestCases))]
        [TestCaseSource(typeof(GeneralTestCases), nameof(GeneralTestCases.PrimitiveCases))]
        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.BoldTests))]
        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.ItalicTests))]
        [TestCaseSource(typeof(BoldItalicTestCases), nameof(BoldItalicTestCases.BoldItalicInteractionTests))]
        public void ConverterShould(string md, string expectedHtml)
        {
            converter.ConvertToHtml(md)
                .Should()
                .Be(expectedHtml);
        }

        private static IEnumerable<TestCaseData> FilesTestCases => ReadTestingFilesFrom(testCasesFolder)
            .Select(x => CreateTestCase(x.FileName, ReadFile(x.Html), ReadFile(x.Md)));

        private static string ReadFile(FileInfo fileInfo)
        {
            using var streamReader = fileInfo.OpenText();
            return streamReader.ReadToEnd();
        }

        private static IEnumerable<(FileInfo Md, FileInfo Html, string FileName)> ReadTestingFilesFrom(
            DirectoryInfo folder) =>
            folder.GetFiles().ToLookup(FileNameOnly)
                .Where(l => l.Count() == 2)
                .Select(l =>
                (
                    Md: l.SingleOrDefault(x => x.Extension == ".md"),
                    Html: l.SingleOrDefault(x => x.Extension == ".html"),
                    FileName: l.Key
                ))
                .Where(x => x.Html != null && x.Md != null);

        private static string FileNameOnly(FileInfo fileInfo) =>
            fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);

        private static TestCaseData CreateTestCase(string testName, string expected, string actual) =>
            new TestCaseData(actual, expected) {TestName = testName};
    }
}