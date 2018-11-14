using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.DataHandlers
{
    public class ItemsConnecter : IConnecter
    {
        private readonly List<int> residualStrengths = new List<int>();
        private readonly Stack<int> indexesLeftSideItems = new Stack<int>();
        private readonly List<List<Connection>> allConnections = new List<List<Connection>>();

        public void AddItem(int maxConnectionStrength, Direction direction)
        {
            RaiseExceptionIfStrengthIsNegative(maxConnectionStrength);
            residualStrengths.Add(maxConnectionStrength);
            allConnections.Add(new List<Connection>());
            if (direction == Direction.Right)
                AddItemInRightDirection(CountItems - 1);
            else if (direction == Direction.Left)
                AddItemInLeftDirection(CountItems - 1);
        }

        private int CountItems => residualStrengths.Count;

        private void AddItemInRightDirection(int indexItem)
        {
            if (residualStrengths[indexItem] == 0)
                return;
            indexesLeftSideItems.Push(indexItem);
        }

        private void AddItemInLeftDirection(int indexItem)
        {
            while (residualStrengths[indexItem] > 0 && indexesLeftSideItems.Any())
            {
                var indexLeftSideItem = indexesLeftSideItems.Pop();
                ConnectTwoItems(indexLeftSideItem, indexItem);
                AddItemInRightDirection(indexLeftSideItem);
            }
        }

        private void ConnectTwoItems(int firstItemIndex, int secondItemIndex)
        {
            var strengthConnection = Math.Min(residualStrengths[firstItemIndex],
                  residualStrengths[secondItemIndex]);
            residualStrengths[firstItemIndex] -= strengthConnection;
            residualStrengths[secondItemIndex] -= strengthConnection;
            var connection = new Connection(firstItemIndex, secondItemIndex, strengthConnection);
            allConnections[firstItemIndex].Add(connection);
        }

        private void RaiseExceptionIfStrengthIsNegative(int strength)
        {
            if (strength < 0)
                throw new ArgumentException("Strength should be a non-negative number");
        }

        public IEnumerable<int> GetResidualStrength()
            => residualStrengths;


        public IEnumerable<Connection> GetSortedConneсtions()
        {
            foreach (var connectionWithItem in allConnections)
            {
                for (var index = connectionWithItem.Count - 1; index >= 0; index--)
                    yield return connectionWithItem[index];
            }
        }
    }
}
