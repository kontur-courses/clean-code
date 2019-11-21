using System.Collections.Generic;
using System.Linq;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;
using NUnit.Framework;

namespace MarkdownTests.TokenReaderTests
{
    public class TokenReaderTestsData
    {
        public static IEnumerable<TestCaseData> ReaderTestCases => GetReaderTestCases();

        private static IEnumerable<TestCaseData> GetReaderTestCases()
        {
            yield return new TestCaseData(@"\\", new List<Token>() {new TextToken(0, @"\")})
                .SetName(@"Escaped \");

            foreach (var tagInfo in TagsUtils.BeginningTagsInfo)
            {
                var mdBeginningTag = tagInfo.MdTag;
                yield return new TestCaseData($"{mdBeginningTag} text",
                        new List<Token>()
                        {
                            new HTMLTagToken(0, mdBeginningTag, HTMLTagType.Header),
                            new SpaceToken(mdBeginningTag.Length, " "),
                            new TextToken(mdBeginningTag.Length + 1, "text")
                        })
                    .SetName($"Simple {tagInfo.TagName}");
            }

            foreach (var tagInfo in TagsUtils.InlineTagsInfo)
            {
                var mdInlineTag = tagInfo.MdTag;
                yield return new TestCaseData($"{mdInlineTag}text text{mdInlineTag}",
                        new List<Token>()
                        {
                            new HTMLTagToken(0, mdInlineTag, HTMLTagType.Opening),
                            new TextToken(mdInlineTag.Length, "text"),
                            new SpaceToken(mdInlineTag.Length + 4, " "),
                            new TextToken(mdInlineTag.Length + 5, "text"),
                            new HTMLTagToken(mdInlineTag.Length + 9, mdInlineTag, HTMLTagType.Closing)
                        })
                    .SetName($"Simple {tagInfo.TagName} with space inside");
                yield return new TestCaseData($"{mdInlineTag}text{mdInlineTag}",
                        new List<Token>()
                        {
                            new HTMLTagToken(0, mdInlineTag, HTMLTagType.Opening),
                            new TextToken(mdInlineTag.Length, "text"),
                            new HTMLTagToken(mdInlineTag.Length + 4, mdInlineTag, HTMLTagType.Closing)
                        })
                    .SetName($"Simple {tagInfo.TagName}");
                yield return new TestCaseData($"{mdInlineTag}text",
                        new List<Token>()
                        {
                            new HTMLTagToken(0, mdInlineTag, HTMLTagType.Opening),
                            new TextToken(mdInlineTag.Length, "text"),
                        })
                    .SetName($"Unpaired {tagInfo.TagName}");

                foreach (var otherTagInfo in TagsUtils.InlineTagsInfo.Where(info => info.TagName != tagInfo.TagName))
                {
                    var otherMdInlineTag = otherTagInfo.MdTag;
                    yield return new TestCaseData(
                            $"{mdInlineTag}t {otherMdInlineTag}t{otherMdInlineTag} t{mdInlineTag}",
                            new List<Token>()
                            {
                                new HTMLTagToken(0, mdInlineTag, HTMLTagType.Opening),
                                new TextToken(mdInlineTag.Length, "t"),
                                new SpaceToken(mdInlineTag.Length + 1, " "),
                                new HTMLTagToken(mdInlineTag.Length + 2, otherMdInlineTag, HTMLTagType.Opening),
                                new TextToken(mdInlineTag.Length + 2 + otherMdInlineTag.Length, "t"),
                                new HTMLTagToken(mdInlineTag.Length + 2 + otherMdInlineTag.Length + 1,
                                    otherMdInlineTag, HTMLTagType.Closing),
                                new SpaceToken(mdInlineTag.Length + 2 + 2 * otherMdInlineTag.Length + 1, " "),
                                new TextToken(mdInlineTag.Length + 2 + 2 * otherMdInlineTag.Length + 2, "t"),
                                new HTMLTagToken(mdInlineTag.Length + 2 + 2 * otherMdInlineTag.Length + 3,
                                    mdInlineTag, HTMLTagType.Closing)
                            })
                        .SetName($"{otherTagInfo.TagName} nested into {tagInfo.TagName}");
                }
            }
        }
    }
}