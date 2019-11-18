using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Markdown.Core;
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
    
    [TestFixture]
    public class MarkdownTests
    {
        private readonly MdRenderer markdown = new MdRenderer();

        [TestCase("_abc __asf fw__ yre_", TestName = "Strong nested into em",
            ExpectedResult = "<em>abc <strong>asf fw</strong> yre</em>")]
        [TestCase("__abc _asf fw_ yre__", TestName = "Em nested into strong",
            ExpectedResult = "<strong>abc <em>asf fw</em> yre</strong>")]
        [TestCase("test_t_e_x__t", TestName = "Underscore inside word", 
            ExpectedResult = "test_t_e_x__t")]        
        [TestCaseSource(typeof(MarkdownTestsData), 
            nameof(MarkdownTestsData.SingleTestCases), Category = "Single tag cases")]
        [TestCaseSource(typeof(MarkdownTestsData), 
            nameof(MarkdownTestsData.UnpairedTestCases), Category = "Unpaired tag cases")]
        public string RenderShouldReturnCorrectHtmlText(string mdText)
        {
            return markdown.Render(mdText);
        }
    }
}