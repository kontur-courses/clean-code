using System;
using System.Collections.Generic;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.Containers
{
    public class  MdConvertedItem
    {
        public readonly Direction Direction;
        public readonly IReadOnlyCollection<MdSelectionType> Selections;
        public readonly int ResidentalStrength;
        public MdConvertedItem(Direction direction, IReadOnlyCollection<MdSelectionType> selections, int residentalStrength)
        {
            Direction = direction;
            Selections = selections;
            ResidentalStrength = residentalStrength;
        }

        private void RaiseIfRemainingStrengthIsIncorrect(int residentalStrength)
        {
            if (residentalStrength < 0)
                throw new ArgumentException("Remaining strength should be a non-negative number");
        }
    }
}
