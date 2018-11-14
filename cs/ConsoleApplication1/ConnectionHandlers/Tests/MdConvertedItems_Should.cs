using System;
using System.Collections.Generic;
using NUnit.Framework;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class MdConvertedItems_Should
    {
        [Test]
        public void Creation_RaisesException_GivenStrengthIsNegative()
        {
            Assert.Throws<ArgumentException>(() =>
                new MdConvertedItem(Direction.BothSides, new List<MdSelectionType>(), -1));
        }

        [Test]
        public void ContainDirection_FromCreation()
        {
            var expectedDirection = Direction.Left;
            var mdConvertedItem = new MdConvertedItem(expectedDirection, new List<MdSelectionType>(), 1);

            mdConvertedItem.Direction.Should().Be(expectedDirection);
        }

        [Test]
        public void ContainSelections_FromCreation()
        {
            var selections = new List<MdSelectionType>() { MdSelectionType.Italic, MdSelectionType.Italic };
            var mdConvertedItem = new MdConvertedItem(Direction.Left, selections, 10);

            mdConvertedItem.Selections.Should().BeEquivalentTo(selections,
                assertionOptions => assertionOptions.WithStrictOrdering());

        }

        [Test]
        public void ContainStrength_FromCreation()
        {
            var strength = 10;
            var mdConvertedItem = new MdConvertedItem(Direction.BothSides, new List<MdSelectionType>(), strength);

            mdConvertedItem.ResidualStrength.Should().Be(strength);
        }

        [Test]
        public void Direction_ShouldNotChange_WhenCreatedAnotherMdConvertedItem()
        {
            var direction = Direction.Left;
            var firstItem = new MdConvertedItem(direction, new List<MdSelectionType>(), 10);
            var secondItem = new MdConvertedItem(Direction.Right, new List<MdSelectionType>(), 11);

            firstItem.Direction.Should().Be(direction);
        }

        [Test]
        public void Selections_ShouldNotChange_WhenCreatedAnotherMdConvertedItem()
        {
            var selections = new[] { MdSelectionType.Italic, MdSelectionType.Bold };
            var firstItem = new MdConvertedItem(Direction.Left, selections, 10);
            var secondItem = new MdConvertedItem(Direction.Right, new List<MdSelectionType>(), 11);

            firstItem.Selections.Should().BeEquivalentTo(selections,
                assertionsOptions => assertionsOptions.WithStrictOrdering());
        }

        [Test]
        public void Strength_ShouldNotChange_WhenCreatedAnotherMdConvertedItem()
        {
            var strength = 10;
            var firstItem = new MdConvertedItem(Direction.Left, new List<MdSelectionType>(), strength);
            var secondItem = new MdConvertedItem(Direction.Left, new List<MdSelectionType>(), 4);

            firstItem.ResidualStrength.Should().Be(strength);
        }

    }
}