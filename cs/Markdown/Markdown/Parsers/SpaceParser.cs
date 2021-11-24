using System.Text;

namespace Markdown.Parsers
{
    internal class SpaceParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder stringBuilder, ref string line, char currentSymbol)
        {
            return new TokenSpace();
        }
    }
}