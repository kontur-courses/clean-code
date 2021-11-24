using Markdown.Tags;
using System.Text;

namespace Markdown.Parsers
{
    public class StartLinkParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line, char currentSymbol)
        {
            return new TagLink(null);
        }
    }
}
