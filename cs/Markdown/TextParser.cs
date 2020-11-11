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
                var letterType = GetTokenType(index, text);
                if (letterType == TokenType.Strong)
                {
                    sb.Append(text[index + 1]);
                    index++;
                    if (sb.Length > 2 && GetTokenType(0,sb.ToString()) == GetTokenType(sb.Length - 2, sb.ToString()))
                    {
                        sb.Remove(0, 2);
                        sb.Remove(sb.Length - 2, 2);
                        var tokenToAdd = new TextToken(index - sb.Length - 1, sb.Length, letterType, sb.ToString());
                        splittedText.Add(tokenToAdd);
                        sb.Clear();
                    }
                }
                else if (letterType == TokenType.Emphasized)
                {
                    if (sb.Length > 1 && GetTokenType(0, sb.ToString()) == GetTokenType(sb.Length - 1, sb.ToString()))
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
                    if (index + 1 == text.Length || (GetTokenType(index + 1,text) != TokenType.Text && GetTokenType(0,sb.ToString()) == TokenType.Text))
                    {
                        var tokenToAdd = new TextToken(index - sb.Length + 1, sb.Length, TokenType.Text, sb.ToString());
                        splittedText.Add(tokenToAdd);
                        sb.Clear();
                    }
                }

            }
            return splittedText;
        }

        public TokenType GetTokenType(int index, string text)
        {
            if (index + 1 < text.Length && text[index] == '_' && text[index + 1] == '_')
                return TokenType.Strong;
            if (text[index] == '_')
                return TokenType.Emphasized;
            if (text[index] == '#')
                return TokenType.Heading;
            return TokenType.Text;
        }


        private TextToken FindOpenedToken(TokenType type, List<TextToken> openedTokens)
        {
            return openedTokens.FirstOrDefault(token => token.Type == type);
        }
    }
}
