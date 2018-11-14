using System;
using System.Collections.Generic;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.Containers
{
    public class  MdConvertedItem
    {
        public readonly Direction Direction;
        public readonly IReadOnlyCollection<MdSelectionType> Selections;
        public readonly int ResidualStrength;
        public MdConvertedItem(Direction direction, IReadOnlyCollection<MdSelectionType> selections, int residualStrength)
        {
            RaiseIfRemainingStrengthIsIncorrect(residualStrength);
            Direction = direction;
            Selections = selections;
            ResidualStrength = residualStrength;
        }

        private void RaiseIfRemainingStrengthIsIncorrect(int residualStrength)
        {
            if (residualStrength < 0)
                throw new ArgumentException("Remaining strength should be a non-negative number");
        }
    }
}
