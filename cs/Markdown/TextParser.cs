using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new NullReferenceException("text was null");

            if (text.Length == 0)
                return tokens;

            for (var index = 0; index < text.Length; index++)
            {
                currentText.Append(text[index]);

                var currentToken = TryGetToken(index, text);
                if (currentToken == null) continue;

                if (currentToken.Type == TokenType.Header)
                {
                    tokens.Add(currentToken);
                    return tokens;
                }
                
                currentText.Clear();

                var updatedToken = UpdateLastTextToken(currentToken);
                if (updatedToken != null) continue;

                tokens.Add(currentToken);
            }

            if (currentText.Length != 0)
                tokens.Add(new TextToken(text.Length - currentText.Length, currentText.Length, TokenType.Text,
                    currentText.ToString()));

            return tokens;
        }

        private TextToken TryGetToken(int index, string text)
        {
            return tokenGetters
                .Where(x => x.CanCreateToken(currentText, text, index))
                .Select(x => x.TryGetToken(currentText, tokenGetters, index, text))
                .FirstOrDefault();
        }

        private TextToken UpdateLastTextToken(TextToken tokenToAdd)
        {
            return tokens.LastOrDefault() != null ? tokens.Last().AddSameToken(tokenToAdd) : null;
        }
    }
}