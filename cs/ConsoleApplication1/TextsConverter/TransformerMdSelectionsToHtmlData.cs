using System;
using System.Collections.Generic;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;

namespace ConsoleApplication1.TextsConverter
{
    public class TransformerMdSelectionsToHtmlData : ITextConverter<TextPart, MdConvertedItem>
    {
        public string ConvertText(IReadOnlyCollection<TextPart> textParts, IReadOnlyCollection<MdConvertedItem> selections)
        {
            throw new NotImplementedException();
        }
    }
}
