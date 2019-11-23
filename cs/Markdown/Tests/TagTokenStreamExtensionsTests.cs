using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownTags;

namespace Markdown
{
    [TestFixture]
    internal class TagTokenStreamExtensionsTests
    {
        private static Dictionary<string, MarkdownTagInfo> TagInfo 
            = new Dictionary<string, MarkdownTagInfo>
            {
                ["emphasisTag"] = new EmphasisTagInfo(),
                ["strongTag"] = new StrongTagInfo(),
            };
        
        [TestCaseSource(nameof(EscapedTagsCases))]
        public void RemoveEscapedDelimiters(string inputString, List<TagToken> tags, int expectedCountTags)
        {
            var correctTags = tags.RemoveEscapedTags(inputString).ToList();
            
            correctTags.Should().HaveCount(expectedCountTags);
        }
        
        private static IEnumerable<TestCaseData> EscapedTagsCases
        {
            get
            {
                yield return new TestCaseData(@"\_a\_", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 1,TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],1, TagTokenType.Closing),
                }, 0).SetName(@"\_a\_");
                yield return new TestCaseData(@"_a_ \_a\_", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 2, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],5, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 8, TagTokenType.Closing),
                }, 2).SetName(@"_a_ \_a\_");
                yield return new TestCaseData(@"\_a_", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 1, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],3, TagTokenType.Closing),
                }, 1).SetName(@"\_a_");
                yield return new TestCaseData(@"_a\_", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 3, TagTokenType.Closing),
                }, 1).SetName(@"_a\_");
                yield return new TestCaseData(@"\__a\__", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 1,TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 5, TagTokenType.Closing),
                }, 0).SetName(@"\__a\__");
                yield return new TestCaseData(@"_a_ \__a\__", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],2, TagTokenType.Closing),
                    new TagToken(TagInfo["strongTag"],5, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],9, TagTokenType.Closing),
                }, 2).SetName(@"_a_ \__a\__");
                yield return new TestCaseData(@"\__a__", new List<TagToken>
                {
                    new TagToken(TagInfo["strongTag"], 1, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"], 4, TagTokenType.Closing),
                }, 1).SetName(@"\__a__");
                yield return new TestCaseData(@"__a\__", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 4, TagTokenType.Closing),
                }, 1).SetName(@"__a\__");
            }
        }

        [TestCaseSource(nameof(UnopenedTagsCases))]
        public void RemoveUnopenedTags(string inputString, List<TagToken> tags, int expectedCountTags)
        {
            var correctTags = tags.OrderBy(tag => tag.Index).RemoveUnpairedTags().ToList();

            correctTags.Should().HaveCount(expectedCountTags);
        }
        
        private static IEnumerable<TestCaseData> UnopenedTagsCases
        {
            get
            {
                yield return new TestCaseData("a_ _a_", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 1, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],3, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 5, TagTokenType.Closing),
                }, 2).SetName("a_ _a_");
                yield return new TestCaseData("a__ a__ a_ a_", new List<TagToken>
                {
                    new TagToken(TagInfo["strongTag"], 1, TagTokenType.Closing),
                    new TagToken(TagInfo["strongTag"],5, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],9, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"], 12, TagTokenType.Closing),
                }, 0).SetName("a__ a__ a_ a_");
                yield return new TestCaseData("__aa a_ aa__", new List<TagToken>
                {
                    new TagToken(TagInfo["strongTag"],0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"], 6, TagTokenType.Closing),
                    new TagToken(TagInfo["strongTag"],10, TagTokenType.Closing),
                }, 2).SetName("__aa a_ aa__");
                yield return new TestCaseData("_aa a__ aa_", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"], 0, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],5, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"], 10, TagTokenType.Closing),
                }, 2).SetName("_aa a__ aa_");
            }
        }
        
        [TestCaseSource(nameof(IncorrectNestingTagsCases))]
        public void RemoveIncorrectNestingTags(string inputString, List<TagToken> tags, int expectedCountTags)
        {
            var correctTags = tags.OrderBy(tag => tag.Index).RemoveIncorrectNestingTags().ToList();

            correctTags.Should().HaveCount(expectedCountTags);
        }
        
        private static IEnumerable<TestCaseData> IncorrectNestingTagsCases
        {
            get
            {
                yield return new TestCaseData("__aa _aa_ aa__", new List<TagToken>
                {
                    new TagToken(TagInfo["strongTag"], 0, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],12, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],5, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],8, TagTokenType.Closing),
                }, 4).SetName("__aa _aa_ aa__");
                yield return new TestCaseData("_aa __aa__ aa_", new List<TagToken>
                {
                    new TagToken(TagInfo["strongTag"],4, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],8, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],13, TagTokenType.Closing),
                } , 2).SetName("_aa __aa__ aa_");
                yield return new TestCaseData("__aa _a_ _a_ aa__", new List<TagToken>
                {
                    new TagToken(TagInfo["emphasisTag"],5, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],9, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],7, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],11, TagTokenType.Closing),
                    new TagToken(TagInfo["strongTag"],0, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],15, TagTokenType.Closing),
                }, 6).SetName("__aa _a_ _a_ aa__");
                yield return new TestCaseData("_aa __a__ __a__ aa_", new List<TagToken>
                {
                    new TagToken(TagInfo["strongTag"],4, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],9, TagTokenType.Opening),
                    new TagToken(TagInfo["strongTag"],7, TagTokenType.Closing),
                    new TagToken(TagInfo["strongTag"],12, TagTokenType.Closing),
                    new TagToken(TagInfo["emphasisTag"],0, TagTokenType.Opening),
                    new TagToken(TagInfo["emphasisTag"],17, TagTokenType.Closing),
                }, 2).SetName("_aa __a__ __a__ aa_");
            }
        }
    }
}
