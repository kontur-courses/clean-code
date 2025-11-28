using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal sealed class EscapeProcessor : ITokenProcessor
    {
        public List<Token> Process(List<Token> tokens)
        {
            var result = new List<Token>();
            for (int i = 0; i < tokens.Count; i++)
            {
                var t = tokens[i];
                if (t.Type != TokenType.Backslash)
                {
                    result.Add(t);
                    continue;
                }
                if(i == tokens.Count - 1 || 
                    (tokens[i + 1].Type != TokenType.DoubleUnderscore &&
                    tokens[i + 1].Type != TokenType.Underscore &&
                    tokens[i + 1].Type != TokenType.Backslash &&
                    tokens[i+1].Type != TokenType.Grid))
                {
                    result.Add(new Token(TokenType.Text, false, false, "\\"));
                    continue;
                }

                result.Add(new Token(TokenType.Text, false, false, tokens[i+1].Text));
                i += 1;
            }

            return result;
        }
    }
}