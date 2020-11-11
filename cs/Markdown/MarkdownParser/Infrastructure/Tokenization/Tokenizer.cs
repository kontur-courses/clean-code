using System.Collections.Generic;
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

        public IEnumerable<Token> Tokenize(string rawInput)
        {
            var textTokenBuilder = new StringBuilder();
            for (var i = 0; i < rawInput.Length; i++)
            {
                var context = new TokenizationContext(rawInput, i, GetPosition(rawInput, i));
                if (TryCreateTokenFrom(context, out var token))
                {
                    if (textTokenBuilder.Length != 0)
                    {
                        yield return CreateDefault(i - textTokenBuilder.Length, textTokenBuilder.ToString());
                        textTokenBuilder.Clear();
                    }

                    yield return token;
                    i += token.RawText.Length - 1; // эти символы мы уже "прошли" внутри TryCreate, пропускаем
                }
                else textTokenBuilder.Append(rawInput[i]);
            }

            if (textTokenBuilder.Length != 0)
                yield return CreateDefault(rawInput.Length - textTokenBuilder.Length, textTokenBuilder.ToString());
        }

        private bool TryCreateTokenFrom(TokenizationContext context, out Token created)
        {
            var validBuilders = tokenBuilders.Where(tb => tb.CanCreateOnPosition(context.TokenPosition)).ToList();
            if (validBuilders.Count == 0)
            {
                created = default;
                return false;
            }

            var textBuilder = new StringBuilder();
            for (var i = context.CurrentStartIndex; i < context.Source.Length; i++)
            {
                var currentText = textBuilder.Append(context.Source[i]).ToString();
                validBuilders.RemoveAll(b => !b.TokenSymbol.StartsWith(currentText)); // TODO optimize & refactor
                if (validBuilders.Count == 1)
                {
                    created = validBuilders[0].Create(context);
                    return true;
                }

                if (validBuilders.Count == 0)
                    break;
            }

            created = default;
            return false;
        }

        private static Token CreateDefault(int startPosition, string rawText) =>
            new TextToken(startPosition, rawText);

        private static TokenPosition GetPosition(string rawInput, int currentIndex)
        {
            var nextIndex = currentIndex + 1;
            var next = nextIndex == rawInput.Length
                ? (char?) null
                : rawInput[nextIndex];

            var previous = currentIndex == 0
                ? (char?) null
                : rawInput[currentIndex - 1];

            return TokenHelpers.GetPosition(previous, next);
        }
    }
}