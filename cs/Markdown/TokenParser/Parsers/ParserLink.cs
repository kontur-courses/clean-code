using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class ParserLink : Parser
    {
        public ParserLink(IEnumerable<IToken> tokens) : base(tokens)
        {
        }
        
        public override TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            var oppositeTokenType = TokenType.BracketClose;
            if (!HasCloseTokenInLine(oppositeTokenType, position + 1))
                return new ParseAsText(Tokens).ParseToken(position);

            if (!IsLink(position, out var closeIndex, out var oppositeCloseIndex))
                return new ParseAsText(Tokens).ParseToken(position);
            var name = new List<TokenTree>();
            position++;
            while (position < closeIndex && Tokens[position].TokenType != oppositeTokenType)
            {
                var component = base.ParseToken(position);
                name.Add(component);
                position += component.Count;
            }

            var link = NextToken(position + 1).Value.Select(x => x.ToString()).ToArray();
            return new TokenTree(new TokenLink().Create(link, 0), name, 5);
        }
        
        private bool IsLink(int position, out int closeIndex, out int oppositeCloseIndex)
        {
            var token = Tokens[position];
            oppositeCloseIndex = 0;
            if (!TryGetFirstIndexOfTokenInLine(TokenType.SquareBracketClose, position, out closeIndex))
                return false;
            if (!TryGetFirstIndexOfTokenInLine(TokenType.BracketOpen, position, out var oppositeOpenIndex))
                return false;
            if (!TryGetFirstIndexOfTokenInLine(TokenType.BracketClose, position, out oppositeCloseIndex, oppositeOpenIndex + 1))
                return false;

            return oppositeOpenIndex == closeIndex  + 1;
        }
    }
}