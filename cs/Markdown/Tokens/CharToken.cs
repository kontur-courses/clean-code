using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class CharToken: IToken
    {
        public char Symbol { get; private set; }

        public readonly bool IsDigit;

        public CharToken(char symbol)
        {
            this.Symbol = symbol;
            this.IsDigit = char.IsDigit(symbol);
        }

        public INode ToNode()
        {
            return new StringNode(Symbol.ToString());
        }
    }
}