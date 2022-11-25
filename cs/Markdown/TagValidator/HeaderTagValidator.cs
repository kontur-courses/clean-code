using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.TagValidator
{
    public class HeaderTagValidator: ITagValidator
    {
        private readonly HashSet<char?> acceptableSymbolsBeforeOpening = new HashSet<char?> { null, '\n' };

        public bool IsValid(string text, ITag tag, SubTagOrder order, int start)
        {
            if (order == SubTagOrder.Opening)
                return IsOpeningValid(text, tag, start);

            return true;
        }

        private bool IsOpeningValid(string text, ITag tag, int start)
        {
            var previousIndex = start - 1;

            var previousSymbol = previousIndex > 0 ? text[previousIndex] : (char?)null;

            return acceptableSymbolsBeforeOpening.Contains(previousSymbol);
        }
    }
}
