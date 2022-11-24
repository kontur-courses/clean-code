using System.Diagnostics;
using FluentAssertions;
using Markdown;
using Markdown.MarkerLogic;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkerTests
    {
        private readonly Marker sut = new( new TagsFinder(),new TagsFilter(), new TagsSwitcher());

        [TestCase("_a_", ExpectedResult = "<em>a</em>", Description = "Emphasis should be placed")]
        [TestCase("__a__", ExpectedResult = "<strong>a</strong>", Description = "Strong should be placed")]
        [TestCase("#a", ExpectedResult = "<h1>a</h1>", Description = "Header should be placed")]
        [TestCase("__a _a_ a__", ExpectedResult = "<strong>a <em>a</em> a</strong>",
            Description = "Emphasis should be placed inside Strong")]
        [TestCase("_a __a__ a_", ExpectedResult = "<em>a __a__ a</em>",
            Description = "Strong should not be placed inside Emphasis")]
        [TestCase("__a _a__ a_", ExpectedResult = "__a _a__ a_",
            Description = "Intersected Strong and Emphasis should not be placed")]
        [TestCase("_a1_", ExpectedResult = "<em>a1</em>", Description = "Can mark whole word with digit")]
        [TestCase("a_a1_a", ExpectedResult = "a_a1_a", Description = "Can't mark part of word with digit")]
        [TestCase(@"\_a\_", ExpectedResult = "_a_", Description = "Escapes should be removed and tags not marked")]
        [TestCase(@"\\_a\\_", ExpectedResult = @"\<em>a\</em>", Description = "Slashes can be escaped")]
        [TestCase("_aa_aa", ExpectedResult = "<em>aa</em>aa", Description = "Can mark word from start to center")]
        [TestCase("a_aa_a", ExpectedResult = "a<em>aa</em>a", Description = "Can mark only center of word")]
        [TestCase("aa_aa_", ExpectedResult = "aa<em>aa</em>", Description = "Can mark word from center to end")]
        [TestCase("a_a a_a", ExpectedResult = "a_a a_a", Description = "Can't mark different words from middles")]
        [TestCase("_aa a_a", ExpectedResult = "_aa a_a",
            Description = "Can't mark different words from start to middle")]
        [TestCase("a_a aa_", ExpectedResult = "a_a aa_", Description = "Can't mark different words from middle to end")]
        [TestCase("_aa", ExpectedResult = "_aa", Description = "Marker without pair should't be marked")]
        [TestCase("_aa \r\n aa_", ExpectedResult = "_aa \r\n aa_",
            Description = "Markers from different paragraphs didnt match")]
        [TestCase("aa_ aa_", ExpectedResult = "aa_ aa_",
            Description = "Marker with space after it can't start marking")]
        [TestCase("_aa _aa", ExpectedResult = "_aa _aa", Description = "Marker with space before it can't end marking")]
        [TestCase("____ __", ExpectedResult = "____ __", Description = "Markers with empty string can't mark it")]
        [TestCase("#__a _a_ a__", ExpectedResult = "<h1><strong>a <em>a</em> a</strong></h1>",
            Description = "Paragraph can have all markers")]
        public string Mark_PutStringWithBasicConditionReplacement_ShouldReplaceItAsSpecified(string input)
        {
            var result = sut.Mark(input);

            return result;
        }

        [Test]
        [Description("(2n)^2 > (n^2)*3")]
        public void
            Mark_MarksTwoStringsWhereOneIsHalfAsLong_TimeSpendedOnShorterMultiplyByThree_ShouldBeLongerThanTimeSpendedOnLongerString()
        {
            var n = @"__a___a___a___a_";
            var n2 = @"__a___a___a___a_ __a___a___a___a_";
            var ingot = @"_aa__a__asd__sd_--_1a__3_a_ __a__b_a_a__a_";
            double time1;
            double time2;

            //прогреваю
            sut.Mark(ingot);
            sut.Mark(ingot);
            sut.Mark(ingot);
            sut.Mark(ingot);
            //прогреваю

            var sw = Stopwatch.StartNew();
            sut.Mark(n2);
            sw.Stop();
            time2 = sw.Elapsed.TotalMilliseconds;
            sw.Restart();
            sut.Mark(n);
            sw.Stop();
            time1 = sw.Elapsed.TotalMilliseconds;

            ((time1 * 3) > time2).Should().BeTrue();
        }

        [TestCase("___aa a_ aa__", ExpectedResult = "<strong><em>aa a</em> aa</strong>")]
        [TestCase("___aa___", ExpectedResult = "<strong><em>aa</em></strong>")]
        public string Mark_PutInconclusiveMarkers_ShouldHaveTendencyToPutEmphasisIntoStrong(string input)
        {
            var result = sut.Mark(input);

            return result;
        }

        [TestCase("_aa___aa__", ExpectedResult = "<em>aa</em><strong>aa</strong>")]
        [TestCase("__aa___aa_", ExpectedResult = "<strong>aa</strong><em>aa</em>")]
        public string Mark_PutInconclusiveMarkers_ShouldHaveTendencyToNotCreateIntersections(string input)
        {
            var result = sut.Mark(input);

            return result;
        }

        [Test]
        public void Mark_PutingChainedIntersections_ShouldIgnoreAllOfThem()
        {
            var text = "__a _a a__ __a a_ a__";

            var result = sut.Mark(text);

            result.Should().Be(text);
        }

        [Test]
        public void Mark_PutingNotIntersectedMarksWithinIntersectedMarks_ShouldMarkIt()
        {
            var text = "__a _a _a a_ a__ a_";
            var expectedResult = "__a <em>a _a a</em> a__ a_";

            var result = sut.Mark(text);

            result.Should().Be(expectedResult);
        }

        [Test]
        public void Mark_PutStringWithEscapeInIntersectedMrkers_ShouldMark_NotEscapedIntersectedMarkers()
        {
            var text = @"__a \_a a__ a_";
            var expectedResult = @"<strong>a _a a</strong> a_";

            var result = sut.Mark(text);

            result.Should().Be(expectedResult);
        }
        [Test]
        public void Mark_PutPictureTag_ShouldTagIt()
        {
            var text = @"a![AA](bb)";
            var expectedResult = @"a<img src=""bb"" alt=""AA"">";

            var result = sut.Mark(text);

            result.Should().Be(expectedResult);
        }
        [Test]
        public void Mark_PutPictureTagWithEscape_ShouldTagItCorrectly()
        {
            var text = @"a\![AA](_bb_) aaaa a![AAA](_bb_)aaaa";
            var expectedResult = @"a![AA](<em>bb</em>) aaaa a<img src=""_bb_"" alt=""AAA"">aaaa";

            var result = sut.Mark(text);

            result.Should().Be(expectedResult);
        }
        [Test]
        public void Mark_PutTagsSimilarToPictureTags_ShouldNotTagIt()
        {
            var text = @"a[AA](bb) aaaa a![AAA](bbaaaa";
            var expectedResult = @"a[AA](bb) aaaa a![AAA](bbaaaa";

            var result = sut.Mark(text);

            result.Should().Be(expectedResult);
        }
    }
}