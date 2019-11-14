using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    public class MarkdownSeparatorHandler
    {
        public static HashSet<string> Separators = new HashSet<string> {"_"};

        public static bool IsSeparator(string text, int position)
        {
            return Separators.Contains(text[position].ToString());
        }

        public static string GetSeparator(string text, int position)
        {
            return text[position].ToString();
        }

        public static bool IsSeparatorValid(string text, int position, bool isFirst)
        {
            var anyNonDigitAround = text.GetNeighborsOfSymbol(position).Any(s => !char.IsDigit(s));
            if (isFirst)
            {
                if (position == 0)
                {
                    return anyNonDigitAround && !IsBeginSeparatorInvalid(text, position);
                }

                return text[position - 1] != '\\' && anyNonDigitAround && !IsBeginSeparatorInvalid(text, position);
            }

            return text[position - 1] != '\\' && anyNonDigitAround && !IsEndSeparatorInvalid(text, position);
        }

        private static bool IsBeginSeparatorInvalid(string text, int position)
        {
            return (position == text.Length - 1 || char.IsWhiteSpace(text[position + 1]));
        }

        private static bool IsEndSeparatorInvalid(string text, int position)
        {
            return char.IsWhiteSpace(text[position - 1]);
        }
    }
}