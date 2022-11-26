using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.TagValidator
{
    public class LineTagValidator : ITagValidator
    {
        private readonly HashSet<char?> acceptableSymbolsBeforeOpening = new HashSet<char?> { null, '\n', ':' };

        public bool IsValid(string text, ITag tag, SubTagOrder order, int start)
        {
            if (order == SubTagOrder.Opening)
                return IsOpeningValid(text, start);

            return true;
        }

        private bool IsOpeningValid(string text, int start)
        {
            if (start == 0)
                return true;

            return acceptableSymbolsBeforeOpening.Contains(text[start - 1]);
        }
    }
}