using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class MdHandlerConnections_Should
    {
        private readonly MdHandlerConnections handlerConnections = new MdHandlerConnections();

        [TestCase(new[] { 10 }, TestName = "one item")]
        [TestCase(new[] { 1, 3, 7 }, TestName = "a couple of items with all different strengths")]
        [TestCase(new[] { 1, 1, 1 }, TestName = "all strengths have the same strength")]
        [TestCase(new[] { 1, 1, 5, 9 }, TestName = "half of strengths has the same value")]
        public void TranslateConnections_ReturnsCorrectResidualStrengths_WhenThereIsNoConnections(int[] residualStrengths)
        {
            var mdConvertedItems = handlerConnections.TranslateConnections(Enumerable.Empty<MarkdownConnection>(), residualStrengths);

            var actualResidualStrength = mdConvertedItems.Select(x => x.ResidualStrength).ToArray();

            actualResidualStrength.Should().BeEquivalentTo(residualStrengths,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [TestCase(new[] { 1 }, TestName = "one strength")]
        [TestCase(new[] { 1, 3, 9 }, TestName = "many strengths")]
        public void TranslateConnections_ReturnsNonDirectedItems_WhenThereIsNoConnections(int[] residualStrengths)
        {
            var mdConvertedItems = handlerConnections.TranslateConnections(Enumerable.Empty<MarkdownConnection>(), residualStrengths);

            mdConvertedItems.Should().OnlyContain(x => x.Direction == Direction.None);
        }

        [TestCase(new[] { 1 }, TestName = "one strength")]
        [TestCase(new[] { 1, 9, 7 }, TestName = "many strengths")]
        [TestCase(new int[] { }, TestName = "no strengths")]
        public void TranslateConnections_ReturnsCorrectCountOfStrengths_WhenThereIsNoConnection(int[] residualStrengths)
        {
            var mdConvertedItems = handlerConnections.TranslateConnections(Enumerable.Empty<MarkdownConnection>(), residualStrengths);
            var expectedLength = residualStrengths.Length;

            mdConvertedItems.Should().HaveCount(expectedLength);
        }

        [TestCase(MarkdownConnectionType.Single, TestName = "Single connection")]
        [TestCase(MarkdownConnectionType.Double, TestName = "Double connection")]
        [TestCase(MarkdownConnectionType.SingleAndDouble, TestName = "Single and double connection")]
        public void TranslateConnections_ChangesDirection_WhenThereIsOneConnectionWithGivenType(MarkdownConnectionType connectionType)
        {
            var residualStrengths = new[] { 1, 1, 1 };
            var connection = new MarkdownConnection(0, 2, connectionType);

            var mdConvertedItems = handlerConnections.TranslateConnections(new[] { connection }, residualStrengths).ToList();

            mdConvertedItems[0].Direction.Should().Be(Direction.Right);
            mdConvertedItems[2].Direction.Should().Be(Direction.Left);
        }

        [TestCase(MarkdownConnectionType.Single, TestName = "Single connection")]
        [TestCase(MarkdownConnectionType.Double, TestName = "Double connection")]
        [TestCase(MarkdownConnectionType.SingleAndDouble, TestName = "Single and double connection")]
        public void TranslateConnections_DoesNotChangeDirectionNonConnectedItems_WhenThereIsOneConnectionWithGivenType(MarkdownConnectionType connectionType)
        {
            var residualStrengths = new[] { 1, 2, 3, 5, 6, 7, 9 };
            var connection = new MarkdownConnection(0, 1, connectionType);

            var mdConvertedItems = handlerConnections.TranslateConnections(new[] { connection }, residualStrengths);

            mdConvertedItems.Skip(2).Should().OnlyContain(x => x.Direction == Direction.None);
        }

        [Test]
        public void TranslateConnection_NotChangesDirection_WhenGetsNonDirectedItem()
        {
            var residualStrengths = new[] { 1, 2, 3, 4 };
            var connection = new MarkdownConnection(0, 1, MarkdownConnectionType.None);

            var mdConvertedItems = handlerConnections.TranslateConnections(new[] { connection }, residualStrengths);

            mdConvertedItems.Should().OnlyContain(x => x.Direction == Direction.None);
        }

        [Test, Timeout(1000)]
        public void TranslateConnections_WorksFast_WhenThereIsManyConnections()
        {
            var countItems = 1000;
            var countRightDirectedItems = countItems / 2;
            var residualStrengths = Enumerable.Range(0, countItems);
            var connections = new List<MarkdownConnection>();

            for (var indexFirstItem = 0; indexFirstItem < countRightDirectedItems; indexFirstItem++)
            {
                for (var indexSecondItem = countRightDirectedItems; indexSecondItem < countItems; indexSecondItem++)
                    connections.Add(new MarkdownConnection(indexFirstItem, indexSecondItem, MarkdownConnectionType.Single));
            }

            handlerConnections.TranslateConnections(connections, residualStrengths).ToList();
        }

        [Test]
        public void TranslateConnections_RaisesArgumentException_WhenGetsTwoDirectedItems()
        {
            var residualStrengths = new[] { 1, 2, 3 };
            var connections = new[] { new MarkdownConnection(0, 1, MarkdownConnectionType.Single),
                new MarkdownConnection(1, 2, MarkdownConnectionType.Single)};

            Assert.Throws<ArgumentException>(() => handlerConnections.TranslateConnections(connections, residualStrengths));

        }

        [Test]
        public void TranslateConnections_RaisesArgumentException_WhenGetsDirectionOutOfGivenItems()
        {
            var residualStrengths = new[] { 1, 2, 3 };
            var connections = new[] { new MarkdownConnection(0, 3, MarkdownConnectionType.Single) };

            Assert.Throws<ArgumentException>(() => handlerConnections.TranslateConnections(connections, residualStrengths));
        }

        [Test]
        public void TranslateConnections_ReturnsItemWithCorrectOrderOfSelections_WhenItemHasManyConnections()
        {
            var residualStrengths = new[] { 1, 2, 3, 4, 5, 6 };
            var itemIndex = 1;
            var connectionsType = new[] { MarkdownConnectionType.Single, MarkdownConnectionType.Double, MarkdownConnectionType.Double };
            var expectedSelectionTypes = new[] { MdSelectionType.Italic, MdSelectionType.Bold, MdSelectionType.Bold };
            var connections = connectionsType.Select((x, i) => Tuple.Create(x, i + 1))
                .Select(x => new MarkdownConnection(itemIndex, itemIndex + x.Item2, x.Item1));

            var actualSelectionTypes = handlerConnections
                .TranslateConnections(connections, residualStrengths)
                .ElementAt(itemIndex)
                .Selections;

            actualSelectionTypes.Should()
                .BeEquivalentTo(expectedSelectionTypes,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [TestCase(MarkdownConnectionType.Single, new[] { MdSelectionType.Italic }, TestName = "Single connection")]
        [TestCase(MarkdownConnectionType.Double, new[] { MdSelectionType.Bold }, TestName = "Double connection")]
        [TestCase(MarkdownConnectionType.SingleAndDouble, new[] { MdSelectionType.Bold, MdSelectionType.Italic }, TestName = "Single and double connection")]
        public void TranslateConnections_TransformsConnection_IntoCorrectSelections(MarkdownConnectionType connectionType, MdSelectionType[] expectedSelections)
        {
            var residualStrengths = new[] { 1, 1 };
            var mdSelectedItems = handlerConnections.TranslateConnections(new[] { new MarkdownConnection(0, 1, connectionType) },
                residualStrengths);

            mdSelectedItems.ElementAt(0).Selections.Should().BeEquivalentTo(expectedSelections,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }
        
    }
}
