using System.Collections.Generic;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;
using NUnit.Framework;

namespace MarkdownTests.TranslatorTests
{
    public class MdToHtmlTranslatorTestsData
    {
        public static IEnumerable<TestCaseData> MdToHtmlTranslatorTestCases => GetMdToHtmlTranslatorTestCases();

        private static IEnumerable<TestCaseData> GetMdToHtmlTranslatorTestCases()
        {
            foreach (var tagInfo in TagsUtils.BeginningTagsInfo)
            {
                var mdTag = tagInfo.MdTag;
                var tagName = tagInfo.TagName;
                yield return new TestCaseData(new List<Token>()
                    {
                        new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Header),
                        new SpaceToken(mdTag.Length, " "),
                        new TextToken(mdTag.Length + 1, "text")
                    })
                    .SetName($"Simple {tagName}")
                    .Returns($"<{tagName}> text</{tagName}>");
            }

            foreach (var tagInfo in TagsUtils.InlineTagsInfo)
            {
                var mdTag = tagInfo.MdTag;
                var tagName = tagInfo.TagName;
                yield return new TestCaseData(new List<Token>()
                {
                    new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Opening),
                    new TextToken(mdTag.Length, "t"),
                    new HTMLTagToken(mdTag.Length + 1, $"{mdTag}", HTMLTagType.Closing)
                })
                    .SetName($"Simple {tagName}")
                    .Returns($"<{tagName}>t</{tagName}>");
            }
        }
    }
}