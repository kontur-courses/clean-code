using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization
{
    public class Tokenizer
    {
        private readonly ICollection<ITokenBuilder> tokenBuilders;

        public Tokenizer(IEnumerable<ITokenBuilder> tokenBuilders)
        {
            this.tokenBuilders = tokenBuilders.ToArray();
        }

        public ICollection<Token> Tokenize(string rawInput)
        {
            var tokens = new List<Token>();
            var textTokenBuilder = new StringBuilder();

            for (var i = 0; i < rawInput.Length; i++)
            {
                var position = GetPosition(rawInput, i);
                var context = new TokenizationContext(rawInput, i, position);

                if (TryCreateTokenFrom(context, out var token))
                {
                    if(textTokenBuilder.Length != 0)
                        tokens.Add(CreateDefault(i - textTokenBuilder.Length, textTokenBuilder.ToString()));
                    tokens.Add(token);
                    i += token.RawText.Length - 1; // эти символы мы уже "прошли" внутри TryCreate, пропускаем
                }
                else textTokenBuilder.Append(rawInput[i]);
            }

            return tokens;
        }

        private bool TryCreateTokenFrom(TokenizationContext context, out Token created)
        {
            var validBuilders = tokenBuilders.Where(tb => tb.CanCreateOnPosition(context.TokenPosition)).ToArray();
            if (validBuilders.Length == 0)
            {
                created = default;
                return false;
            }


            throw new NotImplementedException(); //TODO implement
        }

        private static Token CreateDefault(int startPosition, string rawText) =>
            new TextToken(startPosition, rawText);

        private static TokenPosition GetPosition(string rawInput, int currentIndex)
        {
            TokenPosition position = default;
            if (currentIndex + 1 < rawInput.Length)
                position |= GetNextCharFlags(rawInput[currentIndex + 1]);
            if (currentIndex > 0)
                position |= GetPreviousCharFlags(rawInput[currentIndex - 1]);
            return position;
        }

        private static TokenPosition GetNextCharFlags(char next)
        {
            TokenPosition result = default;
            if (char.IsWhiteSpace(next))
                result |= TokenPosition.BeforeWhitespace;
            if (char.IsDigit(next))
                result |= TokenPosition.BeforeDigit;
            if (char.IsLetter(next))
                result |= TokenPosition.BeforeWord;

            var nextCategory = char.GetUnicodeCategory(next);
            if (nextCategory == UnicodeCategory.LineSeparator || nextCategory == UnicodeCategory.ParagraphSeparator)
                result |= TokenPosition.ParagraphEnd;

            return result;
        }

        private static TokenPosition GetPreviousCharFlags(char previous)
        {
            TokenPosition result = default;
            if (char.IsWhiteSpace(previous))
                result |= TokenPosition.AfterWhitespace;
            if (char.IsDigit(previous))
                result |= TokenPosition.AfterDigit;
            if (char.IsLetter(previous))
                result |= TokenPosition.AfterWord;

            var nextCategory = char.GetUnicodeCategory(previous);
            if (nextCategory == UnicodeCategory.LineSeparator || nextCategory == UnicodeCategory.ParagraphSeparator)
                result |= TokenPosition.ParagraphStart;

            return result;
        }
    }
}