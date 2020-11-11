using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TextParser
    {
        public List<TextToken> GetTextTokens(string text)
        {
            if (text == null)
                throw new ArgumentException("string was null");
            var specialSymbols = new List<char> { '_' };
            var splittedText = new List<TextToken>();
            var openedTokens = new List<TextToken>();
            if (text.Length == 0)
                return splittedText;

            var sb = new StringBuilder();
            for (var index = 0; index < text.Length; index++)
            {
                sb.Append(text[index]);
                var letterType = GetTokenType(text[index]);
                if (letterType != TokenType.Text)
                {
                    if (sb.Length != 1 && GetTokenType(sb[0]) == GetTokenType(sb[sb.Length - 1]))
                    {
                        sb.Remove(0, 1);
                        sb.Remove(sb.Length - 1, 1);
                        var tokenToAdd = new TextToken(index - sb.Length, sb.Length, letterType, sb.ToString());
                        splittedText.Add(tokenToAdd);
                        sb.Clear();
                    }
                }
                else
                {
                    if (index + 1 == text.Length || (GetTokenType(text[index + 1]) != TokenType.Text && GetTokenType(sb[0]) == TokenType.Text))
                    {
                        var tokenToAdd = new TextToken(index - sb.Length + 1, sb.Length, TokenType.Text, sb.ToString());
                        splittedText.Add(tokenToAdd);
                        sb.Clear();
                    }
                }

            }
            return splittedText;
        }

        public TokenType GetTokenType(char element)
        {
            if (element == '_')
                return TokenType.Emphasized;
            return TokenType.Text;
        }

        private TextToken FindOpenedToken(TokenType type, List<TextToken> openedTokens)
        {
            return openedTokens.FirstOrDefault(token => token.Type == type);
        }
    }
}
