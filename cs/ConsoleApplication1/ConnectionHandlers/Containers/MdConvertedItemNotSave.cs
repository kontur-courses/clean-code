using System;
using System.Collections.Generic;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.Containers
{
    public class MdConvertedItemNotSafe
    {
        public Direction Direction = Direction.None;
        public int ResidualStrength;
        public List<MdSelectionType> Selections = new List<MdSelectionType>();
        public MdConvertedItemNotSafe(int residualStrength)
        {
            RaiseIfRemainingStrengthIsIncorrect(residualStrength);
            ResidualStrength = residualStrength;
        }

        private void RaiseIfRemainingStrengthIsIncorrect(int residualStrength)
        {
            if (residualStrength < 0)
                throw new ArgumentException("Remaining strength should be a non-negative number");
        }

        public MdConvertedItem ToSafe()
            => new MdConvertedItem(Direction, Array.AsReadOnly<MdSelectionType>(Selections.ToArray()), ResidualStrength);
    }
}
