using System.Collections.Generic;

namespace Markdown
{
    public class ParseHeader : Parser
    {
        public ParseHeader(IEnumerable<IToken> tokens) : base(tokens) { }

        public override TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            var tree = new TokenTree(token, 1);
            position++;
            while (position < Tokens.Count && Tokens[position].TokenType != TokenType.NewLine)
            {
                var component = base.ParseToken(position);
                position += component.Count;
                tree.Add(component);
            }

            return tree;
        }
    }
}