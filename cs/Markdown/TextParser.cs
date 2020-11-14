using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    /*Не особо нравится текущая реализация, но это чистовой вариант, пока что мысли перенести часть кода в класс
     * TextToken, чтобы код в этом классе не был таким большим и читался легче */

    public class TextParser
    {
        private readonly List<TextToken> tokens = new List<TextToken>();
        private readonly StringBuilder currentText = new StringBuilder();
        private readonly IReadOnlyCollection<ITokenGetter> tokenGetters;

        public TextParser(IReadOnlyCollection<ITokenGetter> tokenGetters)
        {
            this.tokenGetters = tokenGetters;
        }

        public List<TextToken> GetTextTokens(string text)
        {
            if (text == null)
                throw new NullReferenceException("string was null");

            if (text.Length == 0)
                return tokens;

            for (var index = 0; index < text.Length; index++)
            {
                if (text[index] == '\\') //TODO: пофиксить баги
                {
                    currentText.Append(text[index + 1]);
                    index += 1;
                    continue;
                }
                currentText.Append(text[index]);
                TryToGetToken(index, text);

            }

            if (currentText.Length != 0)
            {
                tokens.Add(new TextToken(text.Length - currentText.Length, currentText.Length, TokenType.Text,currentText.ToString()));
            }
            return tokens;
        }

        private void TryToGetToken(int index, string text) //TODO выделить часть с добавлением в главный метод, чтобы соответствовало названию
        {
            foreach (var tokenGetter in tokenGetters)
            {
                var currentToken = tokenGetter.TryGetToken(currentText, tokenGetters, index, text);
                if (currentToken == null) continue;
                tokens.Add(currentToken);
                currentText.Clear();
                return;
            }
        }
    }
}