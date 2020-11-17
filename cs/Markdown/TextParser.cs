using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
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
                throw new NullReferenceException(nameof(text) + " was null");

            if (text.Length == 0)
                return tokens;

            for (var index = 0; index < text.Length; index++)
            {
                currentText.Append(text[index]);

                var currentToken = TryGetToken(index, text);
                if (currentToken == null) continue;
                if (currentToken.Type != TokenType.Text)
                    currentToken.SubTokens = new TextParser(tokenGetters)
                        .GetTextTokens(currentText.ToString());
                if (index < currentToken.StartPosition + currentToken.Length - 1)
                    index = currentToken.StartPosition + currentToken.Length - 1;
                currentText.Clear();

                var updatedToken = UpdateLastTextToken(currentToken);
                if (updatedToken != null) continue;

                tokens.Add(currentToken);
            }

            return tokens;
        }

        private TextToken TryGetToken(int index, string text)
        {
            return tokenGetters
                .Where(x => x.CanCreateToken(currentText, text, index))
                .Select(x => x.GetToken(currentText, tokenGetters, index, text))
                .FirstOrDefault();
        }

        private TextToken UpdateLastTextToken(TextToken tokenToAdd)
        {
            return tokens.LastOrDefault() != null ? tokens.Last().AddSameToken(tokenToAdd) : null;
        }
    }
}