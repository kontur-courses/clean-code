using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class ParagraphStyleTagCollectorTests
    {
        private ParagraphStyleTagCollector<HeaderTag> paragraphStyleTagCollector;

        [SetUp]
        public void SetUp()
        {
            var textWorker = new TextWorker(new[] {'_', '#'});
            paragraphStyleTagCollector = new ParagraphStyleTagCollector<HeaderTag>(textWorker);
        }

        [Test]
        public void CollectTags_WhenTagAtStartOfLine_CollectIt()
        {
            var line = "# abc";

            paragraphStyleTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new HeaderTag(0, 5));
        }

        [Test]
        public void CollectTags_IfTagIsNotAtStartOfLine_ReturnEmptyList()
        {
            var line = "ab # cd";

            paragraphStyleTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_WhenTagIsEscaped_DoesNotCollectIt()
        {
            var line = @"\# abc";

            paragraphStyleTagCollector.CollectTags(line).Should().BeEmpty();
        }
    }
}