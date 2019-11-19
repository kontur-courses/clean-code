using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Core
{
    public class MarkdownSeparatorHandler : ISeparatorHandler
    {
        private static readonly HashSet<string> Separators = new HashSet<string> {"_", "\\_"};

        public bool IsSeparator(string text, int position)
        {
            return Separators
                .Where(s => s.Length <= text.Length - position)
                .Any(s => text.Substring(position, s.Length) == s);
        }

        public int GetSeparatorLength(string text, int position)
        {
            return Separators.OrderBy(s => s.Length).First(s => text.Substring(position, s.Length) == s).Length;
        }

        public string GetSeparatorValue(string text, int position)
        {
            return text.Substring(position, GetSeparatorLength(text, position));
        }
    }
}