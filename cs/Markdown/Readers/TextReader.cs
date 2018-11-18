using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class TextReader : IReader
    {
        private readonly HashSet<char> bannedChars;

        public TextReader(IEnumerable<char> bannedChars)
        {
            this.bannedChars = new HashSet<char>(bannedChars);
        }

        public (IToken, int) ReadToken(string text, int idx, ReadingOptions options)
        {
            var res = new StringBuilder();
            var currentIdx = idx;

            while (currentIdx < text.Length)
            {
                var currentChar = text[currentIdx];
                if (bannedChars.Contains(currentChar)) break;
                res.Append(currentChar);
                currentIdx++;
            }

            if (currentIdx == idx)
                return (null, 0);
            return (new TextToken(res.ToString()), currentIdx - idx);
        }
    }
}