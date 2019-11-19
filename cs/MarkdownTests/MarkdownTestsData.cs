using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTestsData
    {
        private static readonly Dictionary<string, string> TagDict = new Dictionary<string, string>()
        {
            {"__", "strong"},
            {"_", "em"}
        };

        public static IEnumerable SingleTestCases => GetSingleTestCases();
        public static IEnumerable UnpairedTestCases => GetUnpairedTestCases();

        private static IEnumerable GetUnpairedTestCases()
        {
            foreach (var pair in TagDict)
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

        private static IEnumerable<TestCaseData> GetSingleTestCases()
        {
            foreach (var pair in TagDict)
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
    }
}