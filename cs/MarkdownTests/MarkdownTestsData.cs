using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTestsData
    {
        private static readonly Dictionary<string, string> InlineTagDict = new Dictionary<string, string>()
        {
            {"__", "strong"},
            {"_", "em"}
        };
        
        private static readonly Dictionary<string, string> HeaderTagDict = new Dictionary<string, string>()
        {
            {"####", "h4"},
            {"###", "h3"},
            {"##", "h2"},
            {"#", "h1"}
        };

        public static IEnumerable<TestCaseData> SingleInlineTagTestCases => GetSingleInlineTagTestCases();
        public static IEnumerable<TestCaseData> UnpairedInlineTagTestCases => GetUnpairedInlineTagTestCases();

        public static IEnumerable<TestCaseData> HeaderTagTestCases => GetHeaderTagTestCases();

        private static IEnumerable<TestCaseData> GetSingleInlineTagTestCases()
        {
            foreach (var pair in InlineTagDict)
            {
                var mdTag = pair.Key;
                var tagName = pair.Value;
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
            foreach (var pair in InlineTagDict)
            {
                var mdTag = pair.Key;
                var tagName = pair.Value;
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
            foreach (var pair in HeaderTagDict)
            {
                var mdTag = pair.Key;
                var tagName = pair.Value;
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