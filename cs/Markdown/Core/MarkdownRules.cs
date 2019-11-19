using System.Linq;
using Markdown.Extensions;
using Markdown.Rules;

namespace Markdown.Core
{
    public class MarkdownRules : IRules
    {
        public bool IsSeparatorValid(string text, int position, bool isFirst)
        {
            var anyNonDigitAround = text.GetNeighborsOfSymbol(position).Any(s => !char.IsDigit(s));
            return anyNonDigitAround && (isFirst
                       ? IsBeginSeparatorValid(text, position)
                       : IsEndSeparatorValid(text, position));
        }

        private bool IsBeginSeparatorValid(string text, int position)
        {
            return position != text.Length - 1 && !char.IsWhiteSpace(text[position + 1]);
        }

        private bool IsEndSeparatorValid(string text, int position)
        {
            return position != 0 && !char.IsWhiteSpace(text[position - 1]);
        }
    }
}