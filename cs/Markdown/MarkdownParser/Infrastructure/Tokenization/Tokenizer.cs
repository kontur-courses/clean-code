using System;
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
                var context = new TokenizationContext(rawInput, i);
                if (TryCreateTokenFrom(tokenBuilders, context, out var token))
                {
                    if (textTokenBuilder.Length != 0)
                    {
                        yield return CreateDefaultToken(i - textTokenBuilder.Length, textTokenBuilder.ToString());
                        textTokenBuilder.Clear();
                    }

                    yield return token;
                    i += token.RawValue.Length - 1; // эти символы мы уже "прошли" внутри TryCreate, пропускаем
                }
                else textTokenBuilder.Append(rawInput[i]);
            }

            if (textTokenBuilder.Length != 0)
                yield return CreateDefaultToken(rawInput.Length - textTokenBuilder.Length, textTokenBuilder.ToString());
        }

        // TODO rewrite this method to make it more readable
        private static bool TryCreateTokenFrom(ICollection<ITokenBuilder> tokenBuilders, TokenizationContext context,
            out Token token)
        {
            // take text from source until text end, or limit iterations count with max TokenSymbol length
            var upperBound = Math.Min(
                context.CurrentStartIndex + tokenBuilders.Max(tb => tb.TokenSymbol.Length),
                context.Source.Length);
            var toRemove = new List<ITokenBuilder>();
            tokenBuilders = tokenBuilders.ToList();

            ITokenBuilder currentTokenBuilder = null;
            var text = new StringBuilder();
            for (var i = context.CurrentStartIndex; i < upperBound; i++)
            {
                text.Append(context.Source[i]);
                foreach (var builder in tokenBuilders)
                {
                    if (builder.TokenSymbol.StartsWith(text.ToString()))
                    {
                        if (builder.TokenSymbol.Length != text.Length) continue;
                        if (currentTokenBuilder?.TokenSymbol.Length == builder.TokenSymbol.Length)
                            throw new InvalidOperationException($"Multiple matching builders found for {text}");
                        currentTokenBuilder = builder;
                        toRemove.Add(builder);
                    }
                    else
                        toRemove.Add(builder);
                }

                foreach (var builder in toRemove)
                    tokenBuilders.Remove(builder);
                toRemove.Clear();
            }

            if (currentTokenBuilder == null)
            {
                token = default;
                return false;
            }

            if (currentTokenBuilder.CanCreate(context))
                token = currentTokenBuilder.Create(context);
            else
                token = CreateDefaultToken(context.CurrentStartIndex,
                    text.ToString().Substring(0, currentTokenBuilder.TokenSymbol.Length));
            return true;
        }

        private static TextToken CreateDefaultToken(int startPosition, string rawValue) =>
            new TextToken(startPosition, rawValue);

        public static ICollection<Token> MergeTextTokens(IEnumerable<Token> tokens)
        {
            var result = new List<Token>();
            var previousText = new List<TextToken>();
            foreach (var token in tokens)
            {
                if (token is TextToken textToken)
                {
                    previousText.Add(textToken);
                    continue;
                }

                if (previousText.Count != 0)
                {
                    result.Add(CreateMergedToken());
                    previousText.Clear();
                }

                result.Add(token);
            }
            
            if(previousText.Count != 0)
                result.Add(CreateMergedToken());

            return result;

            TextToken CreateMergedToken()
            {
                var text = string.Join(string.Empty, previousText.Select(x => x.RawValue));
                return CreateDefaultToken(previousText.First().StartPosition, text);
            }
        }
    }
}