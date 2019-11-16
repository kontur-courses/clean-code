using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownSeparatorHandler : ISeparatorHandler
    {
        private static readonly HashSet<string> Separators = new HashSet<string> {"_"};
        private const char EscapeCharacter = '\\';

        public bool IsSeparator(string text, int position)
        {
            return Separators.Contains(text[position].ToString());
        }

        public int GetSeparatorLength(string text, int position)
        {
            return 1;
        }

        public bool IsSeparatorValid(string text, int position, bool isFirst)
        {
            var anyNonDigitAround = text.GetNeighborsOfSymbol(position).Any(s => !char.IsDigit(s));
            return anyNonDigitAround && (isFirst
                ? IsBeginSeparatorValid(text, position)
                : IsEndSeparatorValid(text, position));
        }

        private bool IsBeginSeparatorValid(string text, int position)
        {
            return position == 0
                ? position != text.Length - 1 && !char.IsWhiteSpace(text[position + 1])
                : text[position - 1] != EscapeCharacter && position != text.Length - 1 &&
                  !char.IsWhiteSpace(text[position + 1]);
        }

        private bool IsEndSeparatorValid(string text, int position)
        {
            return position != 0 && text[position - 1] != EscapeCharacter && !char.IsWhiteSpace(text[position - 1]);
        }
    }
}