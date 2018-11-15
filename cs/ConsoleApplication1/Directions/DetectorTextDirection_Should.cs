using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.Directions
{
    [TestFixture]
    public class DetectorTextDirection_Should
    {
        private readonly DetectorTextDirection detector = new DetectorTextDirection();

        [TestCase(TextType.WhiteSpaces, TextType.SimpleText, Direction.Right, TestName =  "Text in the right side")]
        [TestCase(TextType.SimpleText, TextType.SimpleText, Direction.BothSides, TestName = "Text in the two directions")]
        [TestCase(TextType.SimpleText, TextType.WhiteSpaces, Direction.Left, TestName =  "text in the left direction")]
        [TestCase(TextType.Empty, TextType.Empty, Direction.None, TestName =  "no text around")]
        public void GetDirection_WorksCorrect_OnGivenArguments(TextType leftType, TextType rightType, Direction expectedDirection)
        {
            detector.GetDirection(leftType, rightType)
                .Should()
                .Be(expectedDirection);
        }
    }
}