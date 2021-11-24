using Markdown.Tags;
using System.Text;

namespace Markdown.Parsers
{
    public class StartLinkParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line)
        {
            if (line[i] != '[')
                return null;
            return new TagLink(null);
        }
    }
}
