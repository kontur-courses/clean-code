using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class ShieldingTagParser : IParser
    {
        private readonly HashSet<char> _specialSymbols = new HashSet<char> { '_', '#', '[', ']' };

        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line)
        {
            if (i + 1 < line.Length && (_specialSymbols.Contains(line[i + 1]) ||
                line[i + 1] == '\\'))
            {
                stringBuilder.Append(line[i + 1]);
                i++;
            }
            else
            {
                stringBuilder.Append(line[i]);
            }
            return new TokenWord(null);
        }
    }
}
