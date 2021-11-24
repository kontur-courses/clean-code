using System.Text;
using Markdown.Tags;

namespace Markdown.Parsers
{
    public class SingleUnderliningParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line)
        {
            if (line[i] != '_')
                return null;
            if (i < line.Length - 1 && char.IsDigit(line[i + 1]) ||
                i > 0 && char.IsDigit(line[i - 1]))
            {
                return null;
            }
            return new TagItalic();
        }
    }
}
