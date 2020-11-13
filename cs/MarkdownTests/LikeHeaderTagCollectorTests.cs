using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class LikeHeaderTagCollectorTests
    {
        private LikeHeaderTagCollector<HeaderTag> likeHeaderTagCollector;

        [SetUp]
        public void SetUp()
        {
            var textWorker = new TextWorker(new[] {'_', '#'});
            likeHeaderTagCollector = new LikeHeaderTagCollector<HeaderTag>(textWorker);
        }

        [Test]
        public void CollectTags_WhenTagAtStartOfLine_CollectIt()
        {
            var line = "# abc";

            likeHeaderTagCollector.CollectTags(line).Should()
                .BeEquivalentTo(new HeaderTag(0, 5));
        }

        [Test]
        public void CollectTags_IfTagIsNotAtStartOfLine_ReturnEmptyList()
        {
            var line = "ab # cd";

            likeHeaderTagCollector.CollectTags(line).Should().BeEmpty();
        }

        [Test]
        public void CollectTags_WhenTagIsEscaped_DoesNotCollectIt()
        {
            var line = @"\# abc";

            likeHeaderTagCollector.CollectTags(line).Should().BeEmpty();
        }
    }
}