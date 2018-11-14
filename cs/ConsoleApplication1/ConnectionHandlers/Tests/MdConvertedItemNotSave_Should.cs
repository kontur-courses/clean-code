using System;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class MdConvertedItemNotSave_Should
    {
        [Test]
        public void Creation_RaisesException_WhenGetsNegativeStrength()
        {
            Assert.Throws<ArgumentException>(() => new MdConvertedItemNotSafe(-1));
        }

        [Test]
        public void Direction_ShouldNotChange_WhenDirectionChangesAtOtherItem()
        {
            var expectedDirection = Direction.Right;
            var firstItem = new MdConvertedItemNotSafe(2);
            firstItem.Direction = expectedDirection;
            var secondItem = new MdConvertedItemNotSafe(3);
            secondItem.Direction = Direction.Left;
            firstItem.Direction.Should().Be(expectedDirection);
        }

        [Test]
        public void Selections_ShouldNotChange_WhenSelectionsChangedAtOtherItem()
        {
            var firstItem = new MdConvertedItemNotSafe(3);
            firstItem.Selections.Add(MdSelectionType.Bold);
            var secondItem = new MdConvertedItemNotSafe(2);
            secondItem.Selections.Add(MdSelectionType.Italic);
            secondItem.Selections.Add(MdSelectionType.Bold);

            firstItem.Selections.Should().HaveCount(1);

        }

        [Test]
        public void ResidualString_ShouldNotChange_WhenCreatedAnotherItem()
        {
            var expectedStrength = 3;
            var firstItem = new MdConvertedItemNotSafe(expectedStrength);
            var secondItem = new MdConvertedItemNotSafe(2);

            firstItem.ResidualStrength.Should().Be(expectedStrength);
        }

        [Test]
        public void ToSafe_ReturnsCorrectItem()
        {
            var selections = new[] {MdSelectionType.Italic, MdSelectionType.Bold};
            var direction = Direction.Left;
            var strength = 3;

            var expectedItem = new MdConvertedItem(direction, selections, strength);
            var notSafeItem = new MdConvertedItemNotSafe(strength);
            notSafeItem.Direction = direction;
            notSafeItem.Selections.AddRange(selections);
            var safeItem = notSafeItem.ToSafe();

            safeItem.Should().BeEquivalentTo(expectedItem,
                assertionsOptions => assertionsOptions.WithStrictOrdering());

        }
    }
}