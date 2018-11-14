using System;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.Extensions;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class ItemsConnecter_Should
    {
        private ItemsConnecter connecter;

        [SetUp]
        public void SetUp()
        {
            connecter = new ItemsConnecter();
        }


        [TestCase(new int[] { }, TestName = "zero items")]
        [TestCase(new [] { 1 }, TestName = "one item")]
        [TestCase(new [] { 1, 2, 3 }, TestName = "items in incremental order")]
        [TestCase(new [] { 1, 4, 2 }, TestName = "items are not sorted")]
        [TestCase(new [] { 1, 1 }, TestName = "there are repeated items")]
        public void GetResidualStrengths_HasTransmittedValuesInSameOrder_WhenAllItemsDirectedInSameSide(int[] transmittedValues)
        {
            var direction = Direction.Right;
            connecter.AddStrengthsWithSameDirection(transmittedValues, direction);
            connecter.GetResidualStrength().ToArray().Should().BeEquivalentTo(transmittedValues,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [TestCase(new [] { -1 }, TestName = "negative number in start")]
        [TestCase(new [] { 10, 10, -5, 2, 3 }, TestName = "negative number inside")]
        [TestCase(new [] { 10, 10, 10, 3, -1 }, TestName = "negative number in the end")]
        public void AddItem_ThrowsException_WhenStrengthConnectionIsNegative(int[] transmittedValues)
        {
            var direction = Direction.Right;

            Assert.Throws<ArgumentException>(() => connecter.AddStrengthsWithSameDirection(transmittedValues, direction));
        }

        [TestCase(0, Direction.Right, TestName = "strength is zero")]
        [TestCase(10, Direction.None, TestName = "direction is none")]
        [TestCase(10, Direction.BothSides, TestName = "direction in both sides")]
        [TestCase(10, Direction.Left, TestName = "direction in left side")]
        [TestCase(10, Direction.Right, TestName = "direction in rigth side")]
        public void AddItem_DoesNotThrowException_WhenGetsCorrectArgument(int strength, Direction direction)
        {
            Assert.DoesNotThrow(() => connecter.AddItem(strength, direction));
        }

        [TestCase(new [] { 1, 1 }, new [] { Direction.Right, Direction.Left }, new [] { 0, 0 }, TestName = "first item is directed in right side and second item is directed in left side")]
        [TestCase(new [] { 1, 1, 2 }, new [] { Direction.Right, Direction.Right, Direction.Left }, new [] { 0, 0, 0 }, TestName = "two items are directed in right side and third one is directed in left side")]
        [TestCase(new [] { 1, 1, 3 }, new [] { Direction.Right, Direction.Right, Direction.Left }, new [] { 0, 0, 1 }, TestName = "item, which is directed in left side, should have positive strength after connection")]
        [TestCase(new [] { 10, 1, 1 }, new [] { Direction.Right, Direction.Left, Direction.Left }, new [] { 8, 0, 0 }, TestName = "item, which is direction in right side, should have positive strength after connection")]
        [TestCase(new [] { 10, 10, 10, 10 }, new [] { Direction.Right, Direction.None, Direction.BothSides, Direction.Left }, new [] { 0, 10, 10, 0 }, TestName = "left and right-directed items should skip both-sides directed and not directed items")]
        [TestCase(new [] { 10, 10, 10 }, new [] { Direction.Left, Direction.Right, Direction.Left }, new [] { 10, 0, 0 }, TestName = "right-side directed items should skip left-side directed items before")]
        [TestCase(new [] { 10, 10, 10 }, new [] { Direction.Right, Direction.Right, Direction.Left }, new [] { 10, 0, 0 }, TestName = "left-side directed items should connect with closest items at first")]
        [TestCase(new [] { 10, 10, 10 }, new [] { Direction.Right, Direction.Left, Direction.Left }, new [] { 0, 0, 10 }, TestName = "right-side directed items should connect with closest items at first")]
        public void GetResidualStrengths_ReturnsCorrectValues_WhenSendsCorrectItemsWithConnections(int[] transmittedValues, Direction[] directions, int[] expectedResidualStrengths)
        {
            for (var index = 0; index < transmittedValues.Length; index++)
                connecter.AddItem(transmittedValues[index], directions[index]);

            connecter.GetResidualStrength().ToArray().Should().BeEquivalentTo(expectedResidualStrengths,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [TestCase(Direction.Left, Direction.Right, TestName = "first item is left-directed and second item is right-directed")]
        [TestCase(Direction.None, Direction.Right, TestName = "no connection between non-directed item and right-directed item")]
        [TestCase(Direction.BothSides, Direction.Right, TestName = "no connection between both-side directed item and non-directed item")]
        [TestCase(Direction.None, Direction.BothSides, TestName = "no connection between both-side directed item and non-directed item")]
        [TestCase(Direction.None, Direction.None, TestName = "no connection between non-directed items")]

        public void GetResidualStrengths_ReturnsTransmittedValues_WhenThereIsNoConnectionBetweenTwoItems(Direction firstItemDirection, Direction secondItemDirection)
        {
            var transmittedStrengths = new [] { 10, 10 };
            connecter.AddItem(transmittedStrengths[0], firstItemDirection);
            connecter.AddItem(transmittedStrengths[1], secondItemDirection);

            connecter.GetResidualStrength().ToArray().Should().BeEquivalentTo(transmittedStrengths,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [Test]
        public void GetSortedConnection_ReturnsCorrectConnection_WhenThereIsOneConnection()
        {
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Left);

            var expectedConnection = new Connection(0, 1, 1);
            connecter.GetSortedConneсtions().First().Should().BeEquivalentTo(expectedConnection);
        }

        [Test]
        public void GetSortedConnections_ReturnsConnectionsFromExternallyNestedToInternalNested_WhenThereAreNestedConnection()
        {
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Left);
            connecter.AddItem(1, Direction.Left);
            var expectedConnections = new [] { new Connection(0, 3, 1), new Connection(1, 2, 1) };

            connecter.GetSortedConneсtions().ToArray().Should().BeEquivalentTo(expectedConnections,
                    assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [Test]
        public void GetSortedConnections_ReturnsConnectionsFromFirstAddedItems_WhenThereAreNoNestedConnection()
        {
            for (var index = 0; index < 2; index++)
            {
                connecter.AddItem(1, Direction.Right);
                connecter.AddItem(1, Direction.Left);
            }
            var expectedConnections = new [] { new Connection(0, 1, 1), new Connection(2, 3, 1) };

            connecter.GetSortedConneсtions().ToArray().Should().BeEquivalentTo(expectedConnections,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [Test]
        public void GetSortedConnections_ReturnsConnectionAtFirstInNestedOrderThanInAddingOrder()
        {
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Left);
            connecter.AddItem(1, Direction.Left);
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Left);

            var expectedConnections = new [] { new Connection(0, 3, 1), new Connection(1, 2, 1), new Connection(4, 5, 1), };

            connecter.GetSortedConneсtions().ToArray().Should().BeEquivalentTo(expectedConnections,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [TestCase(new [] { 0, 0 }, new[] { Direction.Right, Direction.Left }, 0, TestName = "items strengths are zeros")]
        [TestCase(new [] { 1, 1 }, new[] { Direction.Right, Direction.Left }, 1, TestName = "two items are connected")]
        [TestCase(new [] { 1, 1, 1, 1 }, new[] { Direction.Right, Direction.Right, Direction.Left, Direction.Left }, 2, TestName = "two items are connected")]
        [TestCase(new [] { 1, 1, 2 }, new[] { Direction.Right, Direction.Right, Direction.Left }, 2, TestName = "one item connects to two items")]
        [TestCase(new [] { 1, 1, 1, 1, 1, 1 }, new[] { Direction.Right, Direction.Right, Direction.Left, Direction.Left, Direction.Right, Direction.Left }, 3, TestName = "there are nested and not nested connections")]
        [TestCase(new [] { 1, 1 }, new[] { Direction.Right, Direction.Right }, 0, TestName = "there are no connections")]
        public void GetSortedConnection_ReturnsCorrectConnectionCounts(int[] transmittedValues, Direction[] directions, int expectedCount)
        {
            for (var index = 0; index < transmittedValues.Length; index++)
                connecter.AddItem(transmittedValues[index], directions[index]);

            connecter.GetSortedConneсtions().Should().HaveCount(expectedCount);
        }

        [Test, Timeout(1000)]
        public void AddNextItem_WorksFast_WhenAddedManyItemsWithLargeValues()
        {
            var iterationCount = 100000;
            for (var index = 0; index < iterationCount; index++)
                connecter.AddItem(iterationCount, Direction.Right);
            for (var index = 0; index < iterationCount; index++)
                connecter.AddItem(iterationCount, Direction.Left);
        }

        [Test, Timeout(1000)]
        public void GetSortedConnections_WorksFast_WhenAddedManyItemsWithLargeNestingDegree()
        {
            var iterationCount = 100000;
            for (var index = 0; index < iterationCount; index++)
                connecter.AddItem(iterationCount, Direction.Right);
            for (var index = 0; index < iterationCount; index++)
                connecter.AddItem(iterationCount, Direction.Left);

            connecter.GetSortedConneсtions().ToArray();
        }

        [Test, Timeout(1000)]
        public void GetSortedConnections_WorksFast_WhenAddedManyItemsNotNestedItems()
        {
            var iterationCount = 100000;
            for (var index = 0; index < iterationCount; index++)
            {
                connecter.AddItem(iterationCount, Direction.Right);
                connecter.AddItem(iterationCount, Direction.Left);
            }

            connecter.GetSortedConneсtions().ToArray();
        }

        [TestCase(new [] { 10, 10 }, new [] { Direction.Right, Direction.Left}, 0, 10, TestName = "one connection between equals items")]
        [TestCase(new [] { 10, 10, 15}, new [] { Direction.Right, Direction.Right, Direction.Left }, 0, 5, TestName = "external connection strength reduced by internal connections")]
        [TestCase(new [] { 5, 10}, new [] { Direction.Right, Direction.Left }, 0, 5, TestName = "first item in connection has smaller value than second item")]
        [TestCase(new [] { 10, 5 }, new [] { Direction.Right, Direction.Left }, 0, 5, TestName = "first item in connection has bigger value than second item")]
        [TestCase(new [] { (int)1e9, (int)1e9 }, new [] { Direction.Right, Direction.Left }, 0, (int)1e9, TestName = "strength connection is close to max int")]

        public void GetSortedItems_HaveCorrectStrengthConnectionAtGivenIndex(int[] strengths, Direction[] direction, int indexConnection, int expectedStrength)
        {
            for (var index = 0; index < strengths.Length; index++)
                connecter.AddItem(strengths[index], direction[index]);

            var connectionStrength = connecter
                .GetSortedConneсtions()
                .ElementAt(indexConnection)
                .ConnectionStrength;

            connectionStrength.Should().Be(expectedStrength);
        }
        
        [Test]
        public void GetSortedItems_ReturnsCorrectCountItemsOnFirstConnecter_WhenSecondConnectorHasOwnItems()
        {
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Left);

            var secondConnecter = new ItemsConnecter();
            for (var index = 0; index < 1000; index++)
            {
                secondConnecter.AddItem(1, Direction.Left);
                secondConnecter.AddItem(1, Direction.Right);
            }

            connecter.GetSortedConneсtions().Should().HaveCount(1);
        }

        [Test]
        public void GetSortedItems_ReturnsCorrectCountItemsOnSecondConnecter_WhenFirstConnectersHasOwnItems()
        {
            connecter.AddItem(1, Direction.Right);
            connecter.AddItem(1, Direction.Left);

            var secondConnecter = new ItemsConnecter();
            for (var index = 0; index < 1000; index++)
            {
                secondConnecter.AddItem(1, Direction.Right);
                secondConnecter.AddItem(1, Direction.Left);
            }

            secondConnecter.GetSortedConneсtions().Should().HaveCount(1000);
        }

        

    }
}
