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
                
                var currentToken = TryToGetToken(index, text);
                if (currentToken == null) continue;
                
                currentText.Clear();
                
                var updatedToken = UpdateLastTextToken(currentToken);
                if (updatedToken != null) continue;
                
                tokens.Add(currentToken);
                
            }

            if (currentText.Length != 0)
            {
                tokens.Add(new TextToken(text.Length - currentText.Length, currentText.Length, TokenType.Text,
                    currentText.ToString()));
            }

            return tokens;
        }

        private TextToken TryToGetToken(int index, string text)
        {
            return tokenGetters.Select(tokenGetter => tokenGetter
                    .TryGetToken(currentText, tokenGetters, index, text))
                .FirstOrDefault(currentToken => currentToken != null);
        }

        private TextToken UpdateLastTextToken(TextToken tokenToAdd)
        {
            return tokens.LastOrDefault() != null ? tokens.Last().AddSameToken(tokenToAdd) : null;
        }
    }
}