using Markdown.Tags;

namespace Markdown.TagValidator
{
    public class InlineTagValidator : ITagValidator
    {
        public bool IsValid(string text, ITag tag, SubTagOrder order, int start)
        {
            if (order == SubTagOrder.Opening)
                return IsOpeningValid(text, tag, start);

            return IsClosingValid(text, tag, start);
        }

        private bool IsOpeningValid(string text, ITag tag, int start)
        {
            var nextIndex = start + tag.OpeningSubTag.Length;

            var nextSymbol = text[nextIndex];

            return !IsSymbolDigitOrWhiteSpace(nextSymbol);
        }

        private bool IsClosingValid(string text, ITag tag, int start)
        {
            var previousIndex = start - 1;

            var previousSymbol = text[previousIndex];

            return !IsSymbolDigitOrWhiteSpace(previousSymbol);
        }

        private bool IsSymbolDigitOrWhiteSpace(char symbol)
        {
            return char.IsDigit(symbol) || char.IsWhiteSpace(symbol);
        }
    }
}