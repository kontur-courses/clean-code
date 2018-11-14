using System;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class FilterMeaninglessMdConnection_Should
    {
        private FilterMeaninglessMdConnection filter;

        [SetUp]
        public void SetUp()
        {
            filter = new FilterMeaninglessMdConnection();
        }

        [TestCase(0, 1, MarkdownConnectionType.Single, TestName = "send single type connection")]
        [TestCase(0, 2, MarkdownConnectionType.SingleAndDouble, TestName = "send single and double type connection")]
        [TestCase(0, 3, MarkdownConnectionType.Double, TestName = "send double type connection")]
        [TestCase(0, 6, MarkdownConnectionType.None, TestName = "send non-type connection")]
        public void Filter_ReturnsSameTypeItem_WhenGetsOnlyOneItem(int firstIndex, int secondIndex, MarkdownConnectionType type)
        {
            var mdConnection = CreateMdConnection(firstIndex, secondIndex, type);
            var actualConnection = filter.Filter(mdConnection);

            actualConnection.ConnectionType.Should().Be(mdConnection.ConnectionType);
        }

        [TestCase(MarkdownConnectionType.SingleAndDouble, MarkdownConnectionType.Single, MarkdownConnectionType.None, TestName = "one connection added after externally nested double and single connection")]
        [TestCase(MarkdownConnectionType.Double, MarkdownConnectionType.Double, MarkdownConnectionType.Single, TestName = "double connection added after external added double connection")]
        [TestCase(MarkdownConnectionType.Double, MarkdownConnectionType.Single, MarkdownConnectionType.Single, TestName = "single connection added afrer external added double connection")]
        [TestCase(MarkdownConnectionType.Double, MarkdownConnectionType.None, MarkdownConnectionType.None, TestName = "non-type connection added afrer external added double connection")]
        [TestCase(MarkdownConnectionType.Double, MarkdownConnectionType.SingleAndDouble, MarkdownConnectionType.Single, TestName = "double and single connection added afrer external added double connection")]
        [TestCase(MarkdownConnectionType.Single, MarkdownConnectionType.Double, MarkdownConnectionType.None, TestName = "double connection added after external added single connection")]
        [TestCase(MarkdownConnectionType.Single, MarkdownConnectionType.Single, MarkdownConnectionType.None, TestName = "single connection added after external added single connection")]
        [TestCase(MarkdownConnectionType.Single, MarkdownConnectionType.SingleAndDouble, MarkdownConnectionType.None, TestName = "double and single connection added after external added single connection")]
        [TestCase(MarkdownConnectionType.None, MarkdownConnectionType.SingleAndDouble, MarkdownConnectionType.SingleAndDouble, TestName = "one and double connection added after external added non-type connection")]
        public void Filter_ReturnsCorrectTypeItemOnSecondItem_WhenGetsTwoNestedItems(MarkdownConnectionType firstType, MarkdownConnectionType secondType, MarkdownConnectionType expectedType)
        {
            var firstConnection = CreateMdConnection(0, 4, firstType);
            var secondConnection = CreateMdConnection(1, 2, secondType);
            filter.Filter(firstConnection);
            var filteredConnection = filter.Filter(secondConnection);


            filteredConnection.ConnectionType
                .Should()
                .Be(expectedType);
        }

        [Test]
        public void Filter_ReturnsSameTypeOnSecondItem_WhenGetsTwoNotNestedItems()
        {
            var firstConnection = CreateMdConnection(0, 4, MarkdownConnectionType.SingleAndDouble);
            var secondConnection = CreateMdConnection(5, 6, MarkdownConnectionType.SingleAndDouble);
            filter.Filter(firstConnection);
            filter.Filter(secondConnection)
                .ConnectionType
                .Should()
                .Be(MarkdownConnectionType.SingleAndDouble);
        }

        [Test]
        public void Filter_ThrowsException_WhenGetsNullInsteadOfItem()
        {
            Assert.Throws<ArgumentException>(() => filter.Filter(null));
        }

        [Test]
        public void Filter_ReturnsLimitedThirdConnection_WhenFirstConnectionLimitsHim()
        {
            var firstConnection = CreateMdConnection(0, 10, MarkdownConnectionType.SingleAndDouble);
            var secondConnection = CreateMdConnection(1, 8, MarkdownConnectionType.None);
            var thirdConnection = CreateMdConnection(2, 7, MarkdownConnectionType.SingleAndDouble);
            filter.Filter(firstConnection);
            filter.Filter(secondConnection);
            filter.Filter(thirdConnection).ConnectionType.Should().Be(MarkdownConnectionType.None);
        }

        public void Filter_ReturnsUnlimitedThirdConnection_WhenTwoNestedConnectionsDoNotContainIt()
        {
            var firstConnection = CreateMdConnection(0, 10, MarkdownConnectionType.SingleAndDouble);
            var secondConnection = CreateMdConnection(1, 8, MarkdownConnectionType.None);
            var thirdConnection = CreateMdConnection(11, 20, MarkdownConnectionType.SingleAndDouble);
            filter.Filter(firstConnection);
            filter.Filter(secondConnection);
            filter.Filter(thirdConnection).ConnectionType.Should().Be(MarkdownConnectionType.SingleAndDouble);

        }
        [Test]
        public void Filter_LimitsConnectionOnFirstFilter_WhenFirstLimitsHimButSecondDoesNot()
        {
            var firstConnection = CreateMdConnection(0, 10, MarkdownConnectionType.Double);
            filter.Filter(firstConnection);
            var newFilter = new FilterMeaninglessMdConnection();
            filter.Filter(firstConnection)
                .ConnectionType
                .Should().Be(MarkdownConnectionType.Single);
        }

        [Test]
        public void Filter_DoesNotLimitConnectionOnSecondFilter_WhenFirstLimitsHimButSecondDoesNot()
        {
            var firstConnection = CreateMdConnection(0, 10, MarkdownConnectionType.Double);
            filter.Filter(firstConnection);
            var newFilter = new FilterMeaninglessMdConnection();
            newFilter.Filter(firstConnection)
                .ConnectionType
                .Should().Be(MarkdownConnectionType.Double);
        }

        [Test, Timeout(1000)]
        public void Filter_WorksFast_WhenExecutedManyTimesWithNestedConnections()
        {
            var iterationsCount = 100000;
            var startSecondIndex = iterationsCount * 2;
            for (var index = 0; index < iterationsCount; index++)
            {
                var connection = CreateMdConnection(0, startSecondIndex - index, MarkdownConnectionType.Double);
                filter.Filter(connection);
            }
        }

        [Test, Timeout(1000)]
        public void Filter_WorksFast_WhenExecutedManyTimesNotTwistedConnections()
        {
            var iterationsCount = 100000;
            for (var index = 0; index < iterationsCount; index++)
            {
                var startIndex = index * 2;
                var connection = CreateMdConnection(index * 2, index * 2 + 1, MarkdownConnectionType.Double);
            }
        }

        [Test]
        public void Filter_ReturnsItemWithSameFirstIndex()
        {
            var connection = CreateMdConnection(100, 102, MarkdownConnectionType.Double);
            var expectedFirstIndex = filter.Filter(connection).FirstIndex;

            expectedFirstIndex.Should().Be(connection.FirstIndex);
        }

        [Test]
        public void Filter_ReturnsItemWithSameSecondIndex()
        {
            var connection = CreateMdConnection(100, 102, MarkdownConnectionType.Double);
            var expectedFirstIndex = filter.Filter(connection).SecondIndex;

            expectedFirstIndex.Should().Be(connection.SecondIndex);
        }

        private MarkdownConnection CreateMdConnection(int firstIndex, int secondIndex, MarkdownConnectionType type)
        {
            var connection = new Connection(firstIndex, secondIndex, 10);
            return new MarkdownConnection(connection, type);
        }
    }
}
