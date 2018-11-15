using System;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.ConnectionHandlers.Tests
{
    [TestFixture]
    public class TranslatorDirectedItemsToMdSelection_Should
    {
        private ITranslatorDirectedItems<int> translator;
        private readonly Random random = new Random();

        [SetUp]
        public void SetUp()
        {
            translator = new TranslatorDirectedItemsToMdSelection(new ItemsConnecter(),
                new ConverterConnectionsToMarkdown(),
                new FilterMeaninglessMdConnection(),
                new MdHandlerConnections());
        }

        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfConnecter()
        {
            Assert.Throws<ArgumentException>(() => new TranslatorDirectedItemsToMdSelection(null, 
                new ConverterConnectionsToMarkdown(), 
                new FilterMeaninglessMdConnection(), 
                new MdHandlerConnections()));
        }

        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfConverter()
        {
            Assert.Throws<ArgumentException>(() => new TranslatorDirectedItemsToMdSelection(new ItemsConnecter(),
                null,
                new FilterMeaninglessMdConnection(), 
                new MdHandlerConnections()));
        }

        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfFilter()
        {
            Assert.Throws<ArgumentException>(() => new TranslatorDirectedItemsToMdSelection(new ItemsConnecter(),
                new ConverterConnectionsToMarkdown(),
                null,
                new MdHandlerConnections()));
        }

        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfHandlerConnections()
        {
            Assert.Throws<ArgumentException>(() => new TranslatorDirectedItemsToMdSelection(new ItemsConnecter(),
                new ConverterConnectionsToMarkdown(),
                new FilterMeaninglessMdConnection(),
                null));
        }

        [TestCase(0, TestName = "strength is zero")]
        [TestCase(-100, TestName = "strength is negative")]
        public void AddItem_RaisesException_WhenGetsIncorrectStrength(int strength)
        {
            Assert.Throws<ArgumentException>(() => translator.AddItem(strength, Direction.Left));
        }
        

        [Test]
        public void ExtractConvertedItems_ReturnsCorrectCountOfItems_WhenAddedManyItems()
        {
            var countItems = 100;
           
            for (var index = 0; index < countItems; index++)
            {
                translator.AddItem(2, GetRandomDirection());
            }

            translator.ExtractConvertedItems().Should()
                .HaveCount(countItems);
        }

        [Test]
        public void ExtractConvertedItems_ReturnsCorrectCountOfItems_WhenAfterLastExecutingAddedNewItems()
        {
            const int countItems = 100;

            for (var index = 0; index < countItems; index++)
                translator.AddItem(10, GetRandomDirection());
            translator.ExtractConvertedItems().ToList();

            for (var index = 0; index < countItems; index++)
                translator.AddItem(10, GetRandomDirection());

            translator.ExtractConvertedItems().Should().HaveCount(countItems * 2);
        }

        private Direction GetRandomDirection()
        {
            var numberOfDirection = random.Next(4);
            return (Direction)numberOfDirection;
        }

        [Test, Timeout(1000)]
        public void ExtractConvertedItems_WorksFast_WhenAddedManyItems()
        {
            const int countItems = 100000;

            for (var index = 0; index < countItems; index++)
                translator.AddItem(3, GetRandomDirection());

            translator.ExtractConvertedItems().ToArray();
        }

        [Test]
        public void ExtractConvertedItems_ReturnsCorrectOfItemsCountOnFirstTranslator_WhenThereAreTwoTranslators()
        {
            var secondTranslator = new TranslatorDirectedItemsToMdSelection(new ItemsConnecter(),
                new ConverterConnectionsToMarkdown(),
                new FilterMeaninglessMdConnection(),
                new MdHandlerConnections());

            translator.AddItem(1, Direction.Left);
            secondTranslator.AddItem(2, Direction.Right);
            secondTranslator.AddItem(3, Direction.Left);

            translator.ExtractConvertedItems().Should().HaveCount(1);
        }

        [Test]
        public void ExtractConvertedItems_ReturnsCorrectItems_WhenThereAreNestedConnections()
        {
            translator.AddItem(2, Direction.Right);
            translator.AddItem(2, Direction.Right);
            translator.AddItem(5, Direction.Left);

            var expectedItems = new[] {new MdConvertedItem(Direction.Right, new[] { MdSelectionType.Bold}, 0),
            new MdConvertedItem(Direction.Right, new[] {MdSelectionType.Italic }, 0),
            new MdConvertedItem(Direction.Left, new[] {MdSelectionType.Bold, MdSelectionType.Italic }, 1)};

            translator.ExtractConvertedItems().Should().BeEquivalentTo(expectedItems,
                assertionOptions => assertionOptions.WithStrictOrdering());
        }

        [Test]
        public void ExtractConvertedItems_ReturnsNonDirectedItem_WhenThereIsNoConnection()
        {
            translator.AddItem(5, Direction.Left);
            translator.AddItem(6, Direction.Left);
            translator.AddItem(6, Direction.Right);

            translator.ExtractConvertedItems().Should().OnlyContain(x => x.Direction == Direction.None);
        }

        [Test]
        public void ExtractConvertedItems_RemovingRepeatingConnection()
        {
            var countItems = 100;
            for (var index = 0; index < countItems; index++)
                translator.AddItem(100, Direction.Right);
            for (var index = 0; index < countItems; index++)
                translator.AddItem(100, Direction.Left);

            translator.ExtractConvertedItems().Skip(1).Take(countItems - 2)
                .Should().OnlyContain(x => x.Selections.Count == 0);
        }

        [Test]
        public void ExtractConvertedItems_ReturnItemsWithCorrectSelectionsCounts_WhenThereAreNotNestedConnections()
        {
            translator.AddItem(4, Direction.Right);
            translator.AddItem(3, Direction.Left);
            translator.AddItem(4, Direction.Right);
            translator.AddItem(3, Direction.Left);

            translator.ExtractConvertedItems()
                .Should()
                .Contain(x => x.Selections.Count == 2);
        }
    }
}
