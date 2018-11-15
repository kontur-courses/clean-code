using System;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.UsefulEnums;
using FluentAssertions;
using NUnit.Framework;

namespace ConsoleApplication1.TextsConverter
{
    [TestFixture]
    public class ConverterMdSelectionsToHtmlData_Should
    {
        private readonly ConverterMdSelectionsToHtmlData converterMd = new ConverterMdSelectionsToHtmlData();
        [Test]
        public void ConvertText_ReturnsCorrectHtmlString_WhenThereAreSelectionsWithCommonText()
        {
            var textParts = new[]
            {
                new TextPart("__", TextType.SpecialSymbols),
                new TextPart("aaa", TextType.SimpleText),
                new TextPart("__", TextType.SpecialSymbols)
            };
            var mdItems = new[]
            {
                new MdConvertedItem(Direction.Right, new[] {MdSelectionType.Bold}, 0),
                new MdConvertedItem(Direction.Left, new[] {MdSelectionType.Bold}, 0),
            };
            var expectedString = "<strong>aaa</strong>";
            converterMd.ConvertText(textParts, mdItems)
                .Should()
                .Be(expectedString);
        }

        [TestCase(new[] { MdSelectionType.Italic }, 0, Direction.Left, "</em>", TestName = "Italic selection in left direction")]
        [TestCase(new[] { MdSelectionType.Italic }, 0, Direction.Right, "<em>", TestName = "Italic selection in right direction")]
        [TestCase(new[] { MdSelectionType.Bold }, 0, Direction.Right, "<strong>", TestName = "Bold selection in right direction")]
        [TestCase(new[] { MdSelectionType.Bold }, 0, Direction.Left, "</strong>", TestName = "Bold selection in right direction")]
        [TestCase(new[] { MdSelectionType.Italic, MdSelectionType.Bold }, 0, Direction.Right, "<em><strong>", TestName = "bold and italic selections in right direction")]
        [TestCase(new[] { MdSelectionType.Italic, MdSelectionType.Bold }, 0, Direction.Left, "</strong></em>", TestName = "bold and italic selections in left direction")]
        [TestCase(new[] { MdSelectionType.Italic }, 2, Direction.Right, "<em>__", TestName = "selection in right direction with odd underscores")]
        [TestCase(new[] { MdSelectionType.Italic }, 2, Direction.Left, "__</em>", TestName = "selection in left direction with odd underscores")]
        [TestCase(new MdSelectionType[] { }, 2, Direction.Left, "__", TestName = "no selections")]
        public void ConvertText_ConvertOneSelection_ToCorrectHtmlCode(MdSelectionType[] selectionTypes, int residualStrength, Direction direction, string expectedHtmlText)
        {
            var textParts = new[] { new TextPart("a", TextType.SpecialSymbols) };
            var selections = new[] { new MdConvertedItem(direction, selectionTypes, residualStrength) };

            converterMd.ConvertText(textParts, selections)
                .Should()
                .Be(expectedHtmlText);
        }

        [Test]
        public void ConvertText_RaisesException_IfNotEnoughSelection()
        {
            var textParts = new[] {new TextPart("a", TextType.SpecialSymbols)};
            var selections = new MdConvertedItem[] {};

            Assert.Throws<ArgumentException>(() => converterMd.ConvertText(textParts, selections));
        }
    }
}