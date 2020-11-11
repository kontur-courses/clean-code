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
            var splittedText = new List<TextToken>();
            if (text.Length == 0)
                return splittedText;

            var currentText = new StringBuilder();
            for (var index = 0; index < text.Length; index++)
            {
                currentText.Append(text[index]);
                var letterType = GetTokenType(index, text);
                switch (letterType)
                {
                    case TokenType.Strong:
                    {
                        currentText.Append(text[index + 1]);
                        index++;
                        WorkWithStrongText(currentText, splittedText, index, letterType);

                        break;
                    }
                    case TokenType.Emphasized:
                    {
                        WorkWithEmphasizedText(currentText, splittedText, index, letterType);

                        break;
                    }
                    default:
                    {
                        WorkWithText(currentText, splittedText, text, index, letterType);
                        break;
                    }
                }

            }
            return splittedText;
        }

        private void WorkWithStrongText(StringBuilder sb,List<TextToken> splittedText, int index, TokenType letterType)
        {
            if (sb.Length <= 2 || GetTokenType(0, sb.ToString()) != GetTokenType(sb.Length - 2, sb.ToString())) return;
            sb.Remove(0, 2);
            sb.Remove(sb.Length - 2, 2);
            var tokenToAdd = new TextToken(index - sb.Length - 1, sb.Length, letterType, sb.ToString());
            splittedText.Add(tokenToAdd);
            sb.Clear();
        }

        private void WorkWithEmphasizedText(StringBuilder currentText, List<TextToken> splittedText, int index, TokenType letterType)
        {
            if (currentText.Length <= 1 || GetTokenType(0, currentText.ToString()) !=
                GetTokenType(currentText.Length - 1, currentText.ToString())) return;
            currentText.Remove(0, 1);
            currentText.Remove(currentText.Length - 1, 1);
            var tokenToAdd = new TextToken(index - currentText.Length, currentText.Length, letterType, currentText.ToString());
            splittedText.Add(tokenToAdd);
            currentText.Clear();
        }

        private void WorkWithText(StringBuilder currentText, List<TextToken> splittedText, string text, int index, TokenType letterType)
        {
            if (index + 1 != text.Length && (GetTokenType(index + 1, text) == TokenType.Text ||
                                             GetTokenType(0, currentText.ToString()) != TokenType.Text)) return;
            var tokenToAdd = new TextToken(index - currentText.Length + 1, currentText.Length, TokenType.Text, currentText.ToString());
            splittedText.Add(tokenToAdd);
            currentText.Clear();
        }

        private static TokenType GetTokenType(int index, string text)
        {
            if (index + 1 < text.Length && text[index] == '_' && text[index + 1] == '_')
                return TokenType.Strong;
            switch (text[index])
            {
                case '_':
                    return TokenType.Emphasized;
                case '#':
                    return TokenType.Heading;
                default:
                    return TokenType.Text;
            }
        }
    }
}
