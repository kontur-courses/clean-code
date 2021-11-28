using System.Collections.Generic;
using System.Text;
using Markdow.Interfaces;

namespace Markdown.TokenParser.Parsers
{
    public class ParseAsText : Parser, IConcreteParser
    {
        private readonly List<TokenType> availableTokenTypes = new()
            { TokenType.Text, TokenType.SquareBracketClose, TokenType.BracketClose, TokenType.BracketOpen, TokenType.NewLine, TokenType.WhiteSpace };
        
        public ParseAsText(IEnumerable<IToken> tokens) : base(tokens) { }
        
        public override TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            var leaf = new TokenTree(token.Value, 1);
            return leaf;
        }
        
        public TokenTree ParseTokens(int indexStart, int indexEnd)
        {
            var text = new StringBuilder();
            for (var i = indexStart; i < indexEnd; i++)
            {
                text.Append(Tokens[i].Value);
            }

            var leaf = new TokenTree(text.ToString(), indexEnd - indexStart);
            return leaf;
        }

        public bool CanParse(TokenType tokenType) => availableTokenTypes.Contains(tokenType);
    }
}