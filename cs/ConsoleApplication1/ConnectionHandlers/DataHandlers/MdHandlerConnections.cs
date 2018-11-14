using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.DataHandlers
{
    public class MdHandlerConnections : IHandlerConnections<MarkdownConnection, MdConvertedItem>
    {

        private void AddConnections(IEnumerable<MarkdownConnection> connections, List<MdConvertedItemNotSafe> mdItems)
        {
            foreach (var connection in connections.Where(x => x.ConnectionType != MarkdownConnectionType.None))
            {
                RaiseIfIndexesAreIncorrect(connection, mdItems);
                AddDirections(mdItems, connection);
                AddSelections(mdItems, connection);
            }
        }

        private void AddDirections(List<MdConvertedItemNotSafe> mdItems, MarkdownConnection connection)
        {
            DetermineDirection(mdItems, Direction.Right, connection.FirstIndex);
            DetermineDirection(mdItems, Direction.Left, connection.SecondIndex);
        }

        private void AddSelections(List<MdConvertedItemNotSafe> mdItems, MarkdownConnection connection)
        {
            var typeConnection = connection.ConnectionType;
            AddConnection(mdItems, connection.FirstIndex, typeConnection);
            AddConnection(mdItems, connection.SecondIndex, typeConnection);
        }

        private void AddConnection(List<MdConvertedItemNotSafe> mdItems, int index, MarkdownConnectionType type)
        {
            var mdItem = mdItems[index];
            if (type == MarkdownConnectionType.SingleAndDouble)
            {
                mdItem.Selections.Add(MdSelectionType.Bold);
                mdItem.Selections.Add(MdSelectionType.Italic);
            }
            else if (type == MarkdownConnectionType.Single)
                mdItem.Selections.Add(MdSelectionType.Italic);
            else if (type == MarkdownConnectionType.Double)
                mdItem.Selections.Add(MdSelectionType.Bold);
        }

        private void RaiseIfIndexesAreIncorrect(MarkdownConnection connection, List<MdConvertedItemNotSafe> mdItems)
        {
            if (connection.SecondIndex >= mdItems.Count)
                throw new ArgumentException("One of connection is not inside of items array");
        }

        private void DetermineDirection(IReadOnlyList<MdConvertedItemNotSafe> convertedItems, Direction direction, int index)
        {
            var currentDirection = convertedItems[index].Direction;
            if (currentDirection != direction && currentDirection != Direction.None)
                throw new ArgumentException("One item has two differently directed connections");
            convertedItems[index].Direction = direction;
         }

        public IEnumerable<MdConvertedItem> TranslateConnections(IEnumerable<MarkdownConnection> connections, IEnumerable<int> remainingStrengths)
        {
            var mdItems = remainingStrengths.Select(x => new MdConvertedItemNotSafe(x)).ToList();
            AddConnections(connections, mdItems);
            return mdItems.Select(x => x.ToSafe());
        }
    }
}
