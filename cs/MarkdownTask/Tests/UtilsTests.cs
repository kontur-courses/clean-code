using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTask.MarkdownTests
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void IsEscaped_WithEmptyString_ShouldReturnFalse()
        {
            // Arrange
            string text = string.Empty;
            int position = 0;

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsEscaped_WithSingleCharacter_ShouldReturnFalse()
        {
            // Arrange
            string text = "a";
            int position = 0;

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsEscaped_WithEscapedCharacter_ShouldReturnTrue()
        {
            // Arrange
            string text = "\\a";
            int position = 1;

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsEscaped_WithEscapedEscapeCharacter_ShouldReturnTrue()
        {
            // Arrange
            string text = "\\\\a";
            int position = 2;

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsEscaped_WithUnescapedEscapeCharacter_ShouldReturnFalse()
        {
            // Arrange
            string text = "\\\\\\a";
            int position = 3;

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsEscaped_WithCharacterBeforeEscapedCharacters_ShouldReturnFalse()
        {
            // Arrange
            string text = "a\\";
            int position = 1;

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [TestCase(-1)]
        [TestCase(999)]
        [Test]
        public void IsEscaped_IfOutOfRange_ShouldReturnFalse(int position)
        {
            // Arrange
            string text = "a";

            // Act
            bool result = Utils.IsEscaped(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsAfterNotSpace_WithSingleCharacter_ShouldReturnTrue()
        {
            // Arrange
            string text = "a";
            int position = 0;

            // Act
            bool result = Utils.IsAfterNonSpace(text, position);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsAfterNotSpace_WithLetterCharacter_ShouldReturnTrue()
        {
            // Arrange
            string text = "abc";
            int position = 2;

            // Act
            bool result = Utils.IsAfterNonSpace(text, position);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase("a b", 2)]
        public void IsAfterNotSpace_WithSpace_ShouldReturnFalse(string text, int position)
        {
            // Act
            bool result = Utils.IsAfterNonSpace(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsBeforeNonSpace_WithSingleCharacter_ShouldReturnTrue()
        {
            // Arrange
            string text = "a";
            int position = 0;

            // Act
            bool result = Utils.IsBeforeNonSpace(text, position);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsBeforeNonSpace_WithLetterCharacter_ShouldReturnTrue()
        {
            // Arrange
            string text = "abc";
            int position = 0;

            // Act
            bool result = Utils.IsBeforeNonSpace(text, position);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase("a b", 0)]
        public void IsBeforeNonSpace_WithSpace_ShouldReturnFalse(string text, int position)
        {
            // Act
            bool result = Utils.IsBeforeNonSpace(text, position);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void CanSelect_WithSingleWord_ShouldReturnTrue()
        {
            // Arrange
            string text = "aaa_bbb_ccc";
            int start = 3;
            int end = 7;

            // Act
            bool result = Utils.CanSelect(text, start, end);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void CanSelect_WithTwoWordStartInMiddle_ShouldReturnFalse()
        {
            // Arrange
            string text = "aaa_b b_ccc";
            int start = 3;
            int end = 7;

            // Act
            bool result = Utils.CanSelect(text, start, end);

            // Assert
            result.Should().BeFalse();
        }

        [TestCase("aa _b b_ cc", 3, 7)]
        [TestCase("_a b_", 0, 4)]
        public void CanSelect_WithTwoWordStartOnEdge_ShouldReturnTrue(string text, int start, int end)
        {
            // Act
            bool result = Utils.CanSelect(text, start, end);

            // Assert
            result.Should().BeTrue();
        }
    }
}
