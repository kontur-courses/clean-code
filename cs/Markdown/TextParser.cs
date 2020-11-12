using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    /*Не особо нравится текущая реализация, но это чистовой вариант, пока что мысли перенести часть кода в класс
     * TextToken, чтобы код в этом классе не был таким большим и читался легче */

    public class TextParser
    {
        private readonly List<TextToken> splittedText = new List<TextToken>();
        private readonly StringBuilder currentText = new StringBuilder();

        public List<TextToken> GetTextTokens(string text)
        {
            if (text == null)
                throw new ArgumentException("string was null");

            if (text.Length == 0)
                return splittedText;

            for (var index = 0; index < text.Length; index++)
            {
                if (text[index] == '\\') //TODO: пофиксить баги
                {
                    currentText.Append(text[index + 1]);
                    index += 1;
                    continue;
                }

                currentText.Append(text[index]);
                var letterType = GetTokenType(index, text);
                ChooseWithWhichTokenNeedWork(letterType, text, ref index);
            }

            if (currentText.Length != 0)
                WorkWithTextToken(text, text.Length - 1);
            return splittedText;
        }

        private void
            ChooseWithWhichTokenNeedWork(TokenType letterType, string text,
                ref int index) //Знаю, что использование ref - плохо, хотел вынести большую часть кода в отдельный метод, пофикшу в будущем
        {
            switch (letterType)
            {
                case TokenType.Strong:
                {
                    currentText.Append(text[index + 1]);
                    index++;
                    WorkWithStrongToken(index, letterType);

                    break;
                }
                case TokenType.Emphasized:
                {
                    WorkWithEmphasizedToken(index, letterType);

                    break;
                }

                case TokenType.Heading:
                    //TODO
                    break;

                case TokenType.Text:
                    WorkWithTextToken(text, index);
                    break;

                default:
                {
                    throw new ArgumentException("This token does not supported");
                }
            }
        }

        private void WorkWithStrongToken(int index, TokenType letterType)
        {
            if (currentText.Length <= 2 ||
                GetTokenType(0, currentText.ToString())
                != GetTokenType(currentText.Length - 2, currentText.ToString())) return;

            currentText.Remove(0, 2);
            currentText.Remove(currentText.Length - 2, 2);
            AddToSplittedText(index - currentText.Length - 1, letterType);
        }

        private void WorkWithEmphasizedToken(int index, TokenType letterType)
        {
            if (currentText.Length <= 1 || GetTokenType(0, currentText.ToString()) !=
                GetTokenType(currentText.Length - 1, currentText.ToString())) return;
            currentText.Remove(0, 1);
            currentText.Remove(currentText.Length - 1, 1);
            AddToSplittedText(index - currentText.Length, letterType);
        }

        private void WorkWithTextToken(string text, int index) //TODO дать нормальное название
        {
            if (index + 1 != text.Length && (GetTokenType(index + 1, text) == TokenType.Text ||
                                             GetTokenType(0, currentText.ToString()) != TokenType.Text)) return;
            if (!IsShieldedTextAddedAddToTextToken())
            {
                var tokenToAdd = new TextToken(index - currentText.Length + 1, currentText.Length, TokenType.Text,
                    currentText.ToString());
                splittedText.Add(tokenToAdd);
            }

            currentText.Clear();
        }

        private void AddToSplittedText(int length, TokenType tokenType)
        {
            var tokenToAdd = new TextToken(length, currentText.Length, tokenType, currentText.ToString());
            var subTokens = new TextParser().GetTextTokens(tokenToAdd.Text);
            tokenToAdd.SubTokens = subTokens;
            splittedText.Add(tokenToAdd);
            currentText.Clear();
        }

        private bool IsShieldedTextAddedAddToTextToken()
        {
            var lastAddedElement = splittedText.LastOrDefault();
            if (lastAddedElement == null || lastAddedElement.Type != TokenType.Text)
                return false;
            lastAddedElement.Text += currentText.ToString();
            lastAddedElement.Length += currentText.Length;
            return true;
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