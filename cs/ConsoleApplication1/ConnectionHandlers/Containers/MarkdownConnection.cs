using System;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.Containers
{
    public class MarkdownConnection
    {
        public readonly MarkdownConnectionType ConnectionType;
        public readonly int FirstIndex;
        public readonly int SecondIndex;
        

        public MarkdownConnection(Connection connection, MarkdownConnectionType type)
        {
            FirstIndex = connection.FirstItemIndex;
            SecondIndex = connection.SecondItemIndex;
            ConnectionType = type;
        }

        public MarkdownConnection(int firstIndex, int secondIndex, MarkdownConnectionType type)
        {
            RaiseIfIndexesAreIncorrect(firstIndex, secondIndex);
            if (firstIndex > secondIndex)
            {
                FirstIndex = secondIndex;
                SecondIndex = firstIndex;
            }
            else
            {
                FirstIndex = firstIndex;
                SecondIndex = secondIndex;
            }
            ConnectionType = type;
        }

        private void RaiseIfIndexesAreIncorrect(int firstIndex, int secondIndex)
        {
            if (firstIndex == secondIndex)
                throw new ArgumentException("Indexes should be different!");
            if (firstIndex < 0 || secondIndex < 0)
                throw new ArgumentException("Indexes should be non-negative numbers");
        }

        public MarkdownConnection ChangeType(MarkdownConnectionType type)
            => new MarkdownConnection(FirstIndex, SecondIndex, type);
    }
}
