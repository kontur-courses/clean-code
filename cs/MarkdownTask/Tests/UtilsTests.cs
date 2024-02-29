using FluentAssertions;
using NUnit.Framework;
using static MarkdownTask.PairedTagsParser;

namespace MarkdownTask.MarkdownTests
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void IsEscaped_WithEmptyString_ShouldReturnFalse()
        {
            var text = string.Empty;
            var position = 0;

            var result = Utils.IsEscaped(text, position);

            result.Should().BeFalse();
        }

        [Test]
        public void IsEscaped_WithSingleCharacter_ShouldReturnFalse()
        {
            var text = "a";
            var position = 0;

            var result = Utils.IsEscaped(text, position);

            result.Should().BeFalse();
        }

        [Test]
        public void IsEscaped_WithEscapedCharacter_ShouldReturnTrue()
        {
            var text = "\\a";
            var position = 1;

            var result = Utils.IsEscaped(text, position);

            result.Should().BeTrue();
        }

        [Test]
        public void IsEscaped_WithEscapedEscapeCharacter_ShouldReturnTrue()
        {
            var text = "\\\\a";
            var position = 2;

            var result = Utils.IsEscaped(text, position);

            result.Should().BeFalse();
        }

        [Test]
        public void IsEscaped_WithUnescapedEscapeCharacter_ShouldReturnFalse()
        {
            var text = "\\\\\\a";
            var position = 3;

            var result = Utils.IsEscaped(text, position);

            result.Should().BeTrue();
        }

        [Test]
        public void IsEscaped_WithCharacterBeforeEscapedCharacters_ShouldReturnFalse()
        {
            var text = "a\\";
            var position = 1;

            var result = Utils.IsEscaped(text, position);

            result.Should().BeFalse();
        }

        [TestCase(-1)]
        [TestCase(999)]
        [Test]
        public void IsEscaped_IfOutOfRange_ShouldReturnFalse(int position)
        {
            var text = "a";

            var result = Utils.IsEscaped(text, position);

            result.Should().BeFalse();
        }

        [Test]
        public void CanSelect_WithSingleWord_ShouldReturnTrue()
        {
            var text = "aaa_bbb_ccc";
            var start = new Candidate();
            start.position = 3;
            start.edgeType = EdgeType.MIDDLE;

            var end = new Candidate();
            end.position = 7;
            end.edgeType = EdgeType.MIDDLE;

            var result = Utils.CanSelect(text, start, end);

            result.Should().BeTrue();
        }

        [Test]
        public void CanSelect_WithTwoWordStartInMiddle_ShouldReturnFalse()
        {
            var text = "aaa_b b_ccc";
            var start = new Candidate();
            start.position = 3;
            start.edgeType = EdgeType.MIDDLE;

            var end = new Candidate();
            end.position = 7;
            end.edgeType = EdgeType.MIDDLE;

            var result = Utils.CanSelect(text, start, end);

            result.Should().BeFalse();
        }

        [TestCase("aa _b b_ cc", 3, 7)]
        [TestCase("_a b_", 0, 4)]
        public void CanSelect_WithTwoWordStartOnEdge_ShouldReturnTrue(string text, int startPos, int endPos)
        {
            var start = new Candidate();
            start.position = startPos;
            start.edgeType = EdgeType.EDGE;

            var end = new Candidate();
            end.position = endPos;
            end.edgeType = EdgeType.EDGE;
            var result = Utils.CanSelect(text, start, end);

            result.Should().BeTrue();
        }
    }
}
