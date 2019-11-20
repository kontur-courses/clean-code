using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    [TestFixture]
    internal class TagStreamRedactorTests
    {
        [TestCaseSource(nameof(EscapedTagsCases))]
        public void RemoveEscapedDelimiters(string inputString, List<Tag> tags, int expectedCountTags)
        {
            var correctTags = tags.RemoveEscapedTags(inputString).ToList();
            
            correctTags.Should().HaveCount(expectedCountTags);
        }
        
        private static IEnumerable<TestCaseData> EscapedTagsCases
        {
            get
            {
                yield return new TestCaseData(@"\_a\_", new List<Tag>
                {
                    new Tag("_",1,TagType.Opening,1),
                    new Tag("_", 1, TagType.Closing, 1),
                }, 0).SetName(@"\_a\_");
                yield return new TestCaseData(@"_a_ \_a\_", new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, 1),
                    new Tag("_", 2, TagType.Closing, 1),
                    new Tag("_", 5, TagType.Opening, 1),
                    new Tag("_", 8, TagType.Closing, 1),
                }, 2).SetName(@"_a_ \_a\_");
                yield return new TestCaseData(@"\_a_", new List<Tag>
                {
                    new Tag("_", 1, TagType.Opening, 1),
                    new Tag("_", 3, TagType.Closing, 1),
                }, 1).SetName(@"\_a_");
                yield return new TestCaseData(@"_a\_", new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, 1),
                    new Tag("_", 3, TagType.Closing, 1),
                }, 1).SetName(@"_a\_");
                yield return new TestCaseData(@"\__a\__", new List<Tag>
                {
                    new Tag("__",1,TagType.Opening,2),
                    new Tag("__", 5, TagType.Closing, 2),
                }, 0).SetName(@"\__a\__");
                yield return new TestCaseData(@"_a_ \__a\__", new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, 1),
                    new Tag("_", 2, TagType.Closing, 1),
                    new Tag("__", 5, TagType.Opening, 2),
                    new Tag("__", 9, TagType.Closing, 2),
                }, 2).SetName(@"_a_ \__a\__");
                yield return new TestCaseData(@"\__a__", new List<Tag>
                {
                    new Tag("__", 1, TagType.Opening, 2),
                    new Tag("__", 4, TagType.Closing, 2),
                }, 1).SetName(@"\__a__");
                yield return new TestCaseData(@"__a\__", new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, 2),
                    new Tag("_", 4, TagType.Closing, 2),
                }, 1).SetName(@"__a\__");
            }
        }

        [TestCaseSource(nameof(UnopenedTagsCases))]
        public void RemoveUnopenedTags(string inputString, List<Tag> tags, int expectedCountTags)
        {
            var correctTags = tags.OrderBy(tag => tag.Index).RemoveUnopenedTags().ToList();

            correctTags.Should().HaveCount(expectedCountTags);
        }
        
        private static IEnumerable<TestCaseData> UnopenedTagsCases
        {
            get
            {
                yield return new TestCaseData("a_ _a_", new List<Tag>
                {
                    new Tag("_", 1, TagType.Closing, 1),
                    new Tag("_", 3, TagType.Opening, 1),
                    new Tag("_", 5, TagType.Closing, 1),
                }, 2).SetName("a_ _a_");
                yield return new TestCaseData("a__ a__ a_ a_", new List<Tag>
                {
                    new Tag("__", 1, TagType.Closing, 2),
                    new Tag("__", 5, TagType.Closing, 2),
                    new Tag("_", 9, TagType.Closing, 1),
                    new Tag("_", 12, TagType.Closing, 1),
                }, 0).SetName("a__ a__ a_ a_");
                yield return new TestCaseData("__aa a_ aa__", new List<Tag>
                {
                    new Tag("__", 0, TagType.Opening, 2),
                    new Tag("_", 6, TagType.Closing, 1),
                    new Tag("__", 10, TagType.Closing, 2),
                }, 2).SetName("__aa a_ aa__");
                yield return new TestCaseData("_aa a__ aa_", new List<Tag>
                {
                    new Tag("_", 0, TagType.Opening, 1),
                    new Tag("__", 5, TagType.Closing, 2),
                    new Tag("_", 10, TagType.Closing, 1),
                }, 2).SetName("_aa a__ aa_");
            }
        }
        
        [TestCaseSource(nameof(IncorrectNestingTagsCases))]
        public void RemoveIncorrectNestingTags(string inputString, List<Tag> tags, int expectedCountTags)
        {
            var correctTags = tags.OrderBy(tag => tag.Index).RemoveIncorrectNestingTags().ToList();

            correctTags.Should().HaveCount(expectedCountTags);
        }
        
        private static IEnumerable<TestCaseData> IncorrectNestingTagsCases
        {
            get
            {
                yield return new TestCaseData("__aa _aa_ aa__", new List<Tag>
                {
                    new Tag("__", 0, TagType.Opening, 2),
                    new Tag("__", 12, TagType.Closing, 2),
                    new Tag("_",5, TagType.Opening, 1),
                    new Tag("_",8, TagType.Closing, 1),
                }, 4);
                yield return new TestCaseData("_aa __aa__ aa_", new List<Tag>
                {
                    new Tag("__", 4, TagType.Opening, 2),
                    new Tag("__", 8, TagType.Closing, 2),
                    new Tag("_",0, TagType.Opening, 1),
                    new Tag("_",13, TagType.Closing, 1),
                } , 2);
                yield return new TestCaseData("__aa _a_ _a_ aa__", new List<Tag>
                {
                    new Tag("_",5, TagType.Opening, 1),
                    new Tag("_",9, TagType.Opening, 1),
                    new Tag("_",7, TagType.Closing, 1),
                    new Tag("_",11, TagType.Closing, 1),
                    new Tag("__",0, TagType.Opening, 2),
                    new Tag("__",15, TagType.Closing, 2),
                }, 6);
                yield return new TestCaseData("_aa __a__ __a__ aa_", new List<Tag>
                {
                    new Tag("__",4, TagType.Opening, 2),
                    new Tag("__",9, TagType.Opening, 2),
                    new Tag("__",7, TagType.Closing, 2),
                    new Tag("__",12, TagType.Closing, 2),
                    new Tag("_",0, TagType.Opening, 1),
                    new Tag("_",17, TagType.Closing, 1),
                }, 2);
            }
        }
    }
}
