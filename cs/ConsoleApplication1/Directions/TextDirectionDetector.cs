using System;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Directions
{
    public class DetectorTextDirection : IDirectionChooser<TextType>
    {
        public Direction GetDirection(TextType leftType, TextType rightType)
        {
            if (IsItText(leftType))
            {
                return IsItText(rightType)
                    ? Direction.BothSides
                    : Direction.Left;
            }
            return IsItText(rightType)
                ? Direction.Right
                : Direction.None;
        }

        private bool IsItText(TextType type)
            => type == TextType.SimpleText;
    }
}
