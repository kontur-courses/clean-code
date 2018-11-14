using System;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Directions
{
    public class DetectorTextDirection : IDirectionChooser<TextType>
    {
        public Direction GetDirection(TextType leftItem, TextType rightItem)
        {
            throw new NotImplementedException();
        }
    }
}
