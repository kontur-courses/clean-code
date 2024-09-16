using FluentAssertions;
using Markdown;

namespace MarkDownTests;

[TestFixture(TestOf = typeof(StringManipulator))]
public class StringManipulatorTests
{
    [Test]
    public void Should_ReturnInnerStringCorrect()
    {
        var manipulator = new StringManipulator("<html>abc</html>", "abc", 6, 9);

        manipulator.GetInnerString().Should().Be("abc");
    }
    
    [TestCase(-1,0)]
    [TestCase(0,-1)]
    [TestCase(10,5)]
    [TestCase(1,12)]
    [TestCase(12, 12)]
    public void Should_Throw_WhenIndexesInvalid(int startInnerString, int endInnerString)
    {
        Action action = () => new StringManipulator("<em>abc</em>", "_abc_", startInnerString, endInnerString);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Test]
    public void Should_ReplaceInnerStringCorrect()
    {
        var manipulator = new StringManipulator("<html>abc</html>", "abc", 6, 9);
        
        manipulator.ReplaceInnerString("123");

        manipulator.Content.Should().Be("<html>123</html>");
        manipulator.GetInnerString().Should().Be("123");
    }
}