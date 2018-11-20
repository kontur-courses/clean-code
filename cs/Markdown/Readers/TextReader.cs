using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class TextReader : AbstractReader
    {
        private readonly HashSet<char> tokenEndChars;

        public TextReader(IEnumerable<char> tokenEndChars)
        {
            this.tokenEndChars = new HashSet<char>(tokenEndChars);
        }

        public override (IToken token, int read) ReadToken(string text, int offset, ReadingOptions options)
        {
            CheckArguments(text, offset);
            var res = new StringBuilder();
            var currentIdx = offset;

            while (currentIdx < text.Length)
            {
                var currentChar = text[currentIdx];
                if (tokenEndChars.Contains(currentChar)) break;
                res.Append(currentChar);
                currentIdx++;
            }

            if (currentIdx == offset)
                return (null, 0);
            return (new TextToken(res.ToString()), currentIdx - offset);
        }
    }
}