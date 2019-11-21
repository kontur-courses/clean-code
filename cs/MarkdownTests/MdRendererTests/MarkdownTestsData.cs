using System.Collections.Generic;
using Markdown.Core.Infrastructure;
using NUnit.Framework;

namespace MarkdownTests.MdRendererTests
{
    public class MarkdownTestsData
    {
        public static IEnumerable<TestCaseData> SingleInlineTagTestCases => GetSingleInlineTagTestCases();
        public static IEnumerable<TestCaseData> UnpairedInlineTagTestCases => GetUnpairedInlineTagTestCases();

        public static IEnumerable<TestCaseData> HeaderTagTestCases => GetHeaderTagTestCases();

        private static IEnumerable<TestCaseData> GetSingleInlineTagTestCases()
        {
            foreach (var tagInfo in TagsUtils.InlineTagsInfo)
            {
                var mdTag = tagInfo.MdTag;
                var tagName = tagInfo.TagName;
                yield return new TestCaseData($"text {mdTag}test{mdTag}")
                    .SetName($"Simple {tagName}")
                    .Returns($"text <{tagName}>test</{tagName}>");
                yield return new TestCaseData($"text {mdTag}test test{mdTag}")
                    .SetName($"Space inside {tagName}")
                    .Returns($"text <{tagName}>test test</{tagName}>");
                yield return new TestCaseData($"{mdTag}mark\ndown{mdTag}")
                    .SetName($"Line break inside {tagName}")
                    .Returns($"<{tagName}>mark\ndown</{tagName}>");
                yield return new TestCaseData($"\\{mdTag}test\\{mdTag}")
                    .SetName($"Escaped {tagName}")
                    .Returns($"{mdTag}test{mdTag}");
            }
        }
        
        private static IEnumerable<TestCaseData> GetUnpairedInlineTagTestCases()
        {
            foreach (var tagInfo in TagsUtils.InlineTagsInfo)
            {
                var mdTag = tagInfo.MdTag;
                var tagName = tagInfo.TagName;
                yield return new TestCaseData($"{mdTag}text")
                    .SetName($"Unpaired {tagName} in beginning")
                    .Returns($"{mdTag}text");
                yield return new TestCaseData($"text{mdTag}")
                    .SetName($"Unpaired {tagName} in ending")
                    .Returns($"text{mdTag}");
            }
        }
        
        private static IEnumerable<TestCaseData> GetHeaderTagTestCases()
        {
            foreach (var tagInfo in TagsUtils.BeginningTagsInfo)
            {
                var mdTag = tagInfo.MdTag;
                var tagName = tagInfo.TagName;
                yield return new TestCaseData($"{mdTag} absc")
                    .SetName($"Simple {tagName}")
                    .Returns($"<{tagName}> absc</{tagName}>");
                yield return new TestCaseData($"{mdTag}test")
                    .SetName($"Tag {tagName} without space after")
                    .Returns($"<{tagName}>test</{tagName}>");
                yield return new TestCaseData($"\\{mdTag}test")
                    .SetName($"Escaped {tagName}")
                    .Returns($"{mdTag}test");
                yield return new TestCaseData($"inside{mdTag}text")
                    .SetName($"Inside the text {mdTag} do not count as header")
                    .Returns($"inside{mdTag}text");
            }
        }
    }
}