using System.Collections.Generic;
using System.Linq;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Normalizer;
using Markdown.Core.Tokens;
using NUnit.Framework;

namespace MarkdownTests.MdNormalizerTests
{
    public class MdNormalizerTestsData
    {
        public static IEnumerable<TestCaseData> NormalizerTestCases => GetNormalizerTestCases();

        private static readonly List<IgnoreInsideRule> emptyIgnoreRules = new List<IgnoreInsideRule>();

        private static readonly List<IgnoreInsideRule> strongIgnoreInsideRules = new List<IgnoreInsideRule>()
        {
            new IgnoreInsideRule(
                new List<TagInfo>() {TagsUtils.GetTagInfoByTagName("em")},
                TagsUtils.GetTagInfoByTagName("strong"))
        };

        private static IEnumerable<TestCaseData> GetNormalizerTestCases()
        {
            foreach (var tagInfo in TagsUtils.InlineTagsInfo)
            {
                var mdTag = tagInfo.MdTag;
                yield return new TestCaseData(new List<Token>()
                        {
                            new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Opening),
                            new TextToken(mdTag.Length, "test")
                        },
                        new List<Token>()
                        {
                            new TextToken(0, $"{mdTag}"),
                            new TextToken(mdTag.Length, "test")
                        },
                        emptyIgnoreRules)
                    .SetName($"Unpaired {tagInfo.TagName}");


                foreach (var otherTagInfo in TagsUtils.InlineTagsInfo.Where(info => info.TagName != tagInfo.TagName))
                {
                    var otherMdTag = otherTagInfo.MdTag;
                    yield return new TestCaseData(new List<Token>()
                            {
                                new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length, "test"),
                                new SpaceToken(mdTag.Length + 1, " "),
                                new HTMLTagToken(mdTag.Length + 2, $"{otherMdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + otherMdTag.Length + 1,
                                    $"{otherMdTag}", HTMLTagType.Closing)
                            },
                            new List<Token>()
                            {
                                new TextToken(0, $"{mdTag}"),
                                new TextToken(mdTag.Length, "test"),
                                new SpaceToken(mdTag.Length + 1, " "),
                                new HTMLTagToken(mdTag.Length + 2, $"{otherMdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + otherMdTag.Length + 1,
                                    $"{otherMdTag}", HTMLTagType.Closing)
                            },
                            emptyIgnoreRules)
                        .SetName($"{otherTagInfo.TagName} nested into unpaired {tagInfo.TagName} without ignore");

                    var ignoreRules = new List<IgnoreInsideRule>()
                    {
                        new IgnoreInsideRule(
                            new List<TagInfo>() {tagInfo},
                            otherTagInfo)
                    };
                    
                    yield return new TestCaseData(new List<Token>()
                            {
                                new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length, "t"),
                                new SpaceToken(mdTag.Length + 1, " "),
                                new HTMLTagToken(mdTag.Length + 2, $"{otherMdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + otherMdTag.Length + 1,
                                    $"{otherMdTag}", HTMLTagType.Closing)
                            },
                            new List<Token>()
                            {
                                new TextToken(0, $"{mdTag}"),
                                new TextToken(mdTag.Length, "t"),
                                new SpaceToken(mdTag.Length + 1, " "),
                                new HTMLTagToken(mdTag.Length + 2, $"{otherMdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + otherMdTag.Length + 1,
                                    $"{otherMdTag}", HTMLTagType.Closing)
                            },
                            ignoreRules)
                        .SetName($"{otherTagInfo.TagName} nested into unpaired {tagInfo.TagName} with ignore");
                    
                    yield return new TestCaseData(new List<Token>()
                            {
                                new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length, "t"),
                                new SpaceToken(mdTag.Length + 1, " "),
                                new HTMLTagToken(mdTag.Length + 2, $"{otherMdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + otherMdTag.Length + 1,
                                    $"{otherMdTag}", HTMLTagType.Closing),
                                new SpaceToken(mdTag.Length + 2 + 2 * otherMdTag.Length + 1, " "),
                                new TextToken(mdTag.Length + 2 + 2 * otherMdTag.Length + 2, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + 2 * otherMdTag.Length + 3, 
                                    $"{mdTag}", HTMLTagType.Closing)
                            },
                            new List<Token>()
                            {
                                new HTMLTagToken(0, $"{mdTag}", HTMLTagType.Opening),
                                new TextToken(mdTag.Length, "t"),
                                new SpaceToken(mdTag.Length + 1, " "),
                                new TextToken(mdTag.Length + 2, $"{otherMdTag}"),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length, "t"),
                                new TextToken(mdTag.Length + 2 + otherMdTag.Length + 1,$"{otherMdTag}"),
                                new SpaceToken(mdTag.Length + 2 + 2 * otherMdTag.Length + 1, " "),
                                new TextToken(mdTag.Length + 2 + 2 * otherMdTag.Length + 2, "t"),
                                new HTMLTagToken(mdTag.Length + 2 + 2 * otherMdTag.Length + 3, 
                                    $"{mdTag}", HTMLTagType.Closing)
                            },
                            ignoreRules)
                        .SetName($"{otherTagInfo.TagName} nested into {tagInfo.TagName} with ignore");
                }                
            }
        }
    }
}