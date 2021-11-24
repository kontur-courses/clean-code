using System.Text;
using Markdown.Tags;

namespace Markdown.Parsers
{
    public class DoubleUnderliningParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line)
        {
            if (i + 1 == line.Length || line[i] != '_' || line[i + 1] != '_')
                return null;
            i++;
            return new TagBold();
        }
    }
}
