using System;
using ConsoleApplication1.ConnectionHandlers.Containers;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class Connection_Should
    {
        [TestCase(0, 1, 0, TestName="Strength is zero")]
        [TestCase(0, 1, -1, TestName = "Strength is negative")]
        [TestCase(-1, 1, 3, TestName = "First index is negative")]
        [TestCase(1, -1, 4, TestName = "Second index is negative")]
        [TestCase(1, 1, 5, TestName = "Indexes are the same")]
        public void Creation_Fails_WhenGetsIncorrectArguments(int firstIndex, int secondIndex, int strengthConnection)
        {
            Assert.Throws<ArgumentException>(() => new Connection(firstIndex, secondIndex, strengthConnection));
        }

        [Test]
        public void ReverseIndexes_WhenTheyWereGivenInWrongOrder()
        {
            var firstIndex = 5;
            var secondIndex = 3;
            var strengthConnection = 10;
            var connection = new Connection(firstIndex, secondIndex, strengthConnection);

            connection.SecondItemIndex.Should().Be(firstIndex);
            connection.FirstItemIndex.Should().Be(secondIndex);
        }

        [Test]
        public void FirstItemIndex_ShouldBeCorrect_WhenItIsCreated()
        {
            var firstIndex = 5;
            var secondIndex = 10;
            var connection = new Connection(firstIndex, secondIndex, 10);

            connection.FirstItemIndex.Should().Be(firstIndex);
        }

        [Test]
        public void SecondItemIndex_ShouldBeCorrect_WhenItIsCreated()
        {
            var firstIndex = 5;
            var secondIndex = 10;
            var connection = new Connection(firstIndex, secondIndex, 10);

            connection.SecondItemIndex.Should().Be(secondIndex);
        }

        [Test]
        public void ConnectionStrength_ShouldBeCorrect_WhenItIsCreated()
        {
            var strengthConnection = 20;
            var connection = new Connection(1, 3, strengthConnection);

            connection.ConnectionStrength.Should().Be(strengthConnection);
        }

        [Test]
        public void ConnectionStrengths_Differs_WhenCreatedTwoConnectionsWithDifferentStrengths()
        {
            var firstConnection = new Connection(0, 1, 10);
            var secondConnection = new Connection(0, 1, 20);

            firstConnection.ConnectionStrength
                .Should().NotBe(secondConnection.ConnectionStrength);
        }

        [Test]
        public void FirstItemIndexes_Differs_WhenCreatedTwoConnectionsWithDifferentFirstIndexes()
        {
            var firstConnection = new Connection(1, 10, 10);
            var secondConnection = new Connection(0, 10, 10);

            firstConnection.FirstItemIndex
                .Should().NotBe(secondConnection.FirstItemIndex);
        }

        [Test]
        public void SecondItemIndexes_Differs_WhenCreatedTwoConnectionsWithDifferentSecondIndexes()
        {
            var firstConnection = new Connection(0, 10, 10);
            var secondConnection = new Connection(0, 9, 10);

            firstConnection.SecondItemIndex
                .Should().NotBe(secondConnection.SecondItemIndex);
        }

        [Test]
        public void ContainsCorrectStrength_WhenGivenStrenghtIsCloseToMaxInt()
        {
            var strength = (int)1e9;
            var connection = new Connection(0, 1, strength);

            connection.ConnectionStrength.Should().Be(strength);
        }

        [Test]
        public void ContainsCorrectIndex_WhenGivenIndexIsCloseToMaxInt()
        {
            var index = (int)1e9;
            var connection = new Connection(0, index, 1);

            connection.SecondItemIndex.Should().Be(index);
        }
    }
}
