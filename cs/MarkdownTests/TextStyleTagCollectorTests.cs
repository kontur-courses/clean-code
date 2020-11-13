using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TextStyleTagCollectorTests
    {
        private TextStyleTagCollector<ItalicTag> italicTagCollector;
        private TextStyleTagCollector<BoldTag> boldTagCollector;

        [SetUp]
        public void SetUp()
        {
            var textWorker = new TextWorker(new[] {'_'});
            italicTagCollector = new TextStyleTagCollector<ItalicTag>(textWorker);
            boldTagCollector = new TextStyleTagCollector<BoldTag>(textWorker);
        }

        [Test]
        public void CollectTags_WhenOneValidTagInLine_CollectIt()
        {
            var line = "_tag_";

            italicTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new ItalicTag(0, 4));
        }

        [Test]
        public void CollectTags_WhenMoreThanOneValidTag_CollectAll()
        {
            var line = "_tagA_ _tagB_";

            italicTagCollector.CollectTags(line).Should().BeEquivalentTo(
                new ItalicTag(0, 5),
                new ItalicTag(7, 12));
        }

        [Test]
        public void CollectTags_CorrectWorkWithCompositeTag()
        {
            var line = "__tag__";

            boldTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new BoldTag(0, 5));
        }

        [Test]
        public void CollectTags_WhenThereIsEmptyStringInsideTag_DoesNotCollectIt()
        {
            var line = "ab ____ cd";

            boldTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_WhenNotPairedTag_InterpretBoldTagAsItalic()
        {
            var line = "__ab cd_";

            italicTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new ItalicTag(0, 7));
        }

        [Test]
        public void CollectTags_WhenDigitInTag_DoesNotCollectIt()
        {
            var line = "_ab1_";

            italicTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_DoesNotCollectEscapedTags()
        {
            var line = "ab \\_cd_";

            italicTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_WhenTagsInsideDifferentWords_NotCollectIt()
        {
            var line = "a_b cc_d";

            italicTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_WhenTagsAtBorderOfDifferentWords_CollectIt()
        {
            var line = "_ab cd_";

            italicTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new ItalicTag(0, line.Length - 1));
        }

        [Test]
        public void CollectTags_WhenWhiteSpaceBeforeClosingTag_NotCollectIt()
        {
            var line = "_ab _";

            italicTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_WhenWhiteSpaceAfterOpenTag_NotCollectIt()
        {
            var line = "_ ab_";

            italicTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [TestCase("_sta_rt", 0, 4, TestName = "AtStart")]
        [TestCase("mi_ddl_e", 2, 6, TestName = "AtMiddle")]
        [TestCase("en_d_", 2, 4, TestName = "AtEnd")]
        public void CollectTags_WhenTagsAtDifferentPartsOfOneWord_CollectIt(
            string line, int start, int end)
        {
            italicTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new ItalicTag(start, end));
        }
    }
}