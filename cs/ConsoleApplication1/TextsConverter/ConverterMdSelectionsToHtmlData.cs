using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.TextsConverter
{
    public class ConverterMdSelectionsToHtmlData : ITextConverter<TextPart, MdConvertedItem>
    {
        private readonly IReadOnlyDictionary<MdSelectionType, string> selectionsConformity = new Dictionary<MdSelectionType, string> { { MdSelectionType.Bold, "strong" }, { MdSelectionType.Italic, "em" } };

        public string ConvertText(IReadOnlyCollection<TextPart> textParts, IReadOnlyCollection<MdConvertedItem> selections)
            => string.Concat(ExecuteConversion(textParts, selections.ToArray()));

        private IEnumerable<string> ExecuteConversion(IReadOnlyCollection<TextPart> textParts, MdConvertedItem[] selections)
        {
            var selectionsIndex = 0;
            foreach (var textPart in textParts)
            {
                yield return textPart.Type != TextType.SpecialSymbols
                    ? textPart.Text
                    : GetSelectionAt(selections, selectionsIndex++);
            }
        }

        private string GetSelectionAt(MdConvertedItem[] selections, int indexSelection)
        {
            if (indexSelection < selections.Length)
                return TransformSelection(selections[indexSelection]);
            throw new ArgumentException("Selections do not contain enough items to convert text");
        }

        private string TransformSelection(MdConvertedItem item)
        {
            var transformedSelection = new string('_', item.ResidualStrength);
            var itemDirection = item.Direction;
            switch (itemDirection)
            {
                case Direction.Right:
                    transformedSelection = ConvertSelectionsToHtmlTags(item.Selections, false, "") + transformedSelection;
                    break;
                case Direction.Left:
                    transformedSelection = transformedSelection + ConvertSelectionsToHtmlTags(item.Selections, true, "/");
                    break;
            }
            return transformedSelection;
        }

        private string ConvertSelectionsToHtmlTags(IEnumerable<MdSelectionType> selections, bool reverse, string htmlAddition)
        {
            var currentSelection = reverse
                ? selections.Reverse()
                : selections;
            return string.Concat(currentSelection
                    .Select(x => "<" + htmlAddition + selectionsConformity[x] + ">"));
        }
    }
}
