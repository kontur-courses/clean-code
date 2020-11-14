using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown
{
    /// <summary>
    /// Воркер, который собирает всякие штуки по токенам
    /// </summary>
    public class MarkdownCollector
    {
        private readonly ICollection<IMarkdownElementFactory> providers;

        public MarkdownCollector(IEnumerable<IMarkdownElementFactory> providers)
        {
            this.providers = providers.ToArray();
            foreach (var dependentProvider in this.providers.OfType<IMarkdownCollectorDependent>())
                dependentProvider.SetCollector(this);
        }

        public bool TryCollectUntil<TToken>(MarkdownElementContext currentContext, Predicate<TToken> predicate,
            out TToken matchedToken, out ICollection<Token> collectedTokens) where TToken : Token
        {
            collectedTokens = new List<Token>();
            foreach (var token in currentContext.NextTokens)
            {
                if (token is TToken t && predicate.Invoke(t))
                {
                    matchedToken = t;
                    return true;
                }

                collectedTokens.Add(token);
            }

            matchedToken = default;
            return false;
        }

        public IEnumerable<MarkdownElement> CreateElementsFrom(params Token[] tokens)
        {
            //TODO class Subarray to avoid create new Token[] instance
            for (var i = 0; i < tokens.Length;)
            {
                var token = tokens[i];
                var currentContext = new MarkdownElementContext(token, tokens.Skip(i + 1));
                if (token is TextToken || !TryCreateElementFrom(currentContext, out var element))
                    element = new MarkdownText(token);
                yield return element;
                i += element.Tokens.Length;
            }
        }

        private bool TryCreateElementFrom(MarkdownElementContext currentContext, out MarkdownElement elem)
        {
            var parsed = providers.Select(p => GetParsedOrNull(p, currentContext))
                .Where(x => x != null)
                .ToArray();

            if (parsed.Length > 1)
                throw new InvalidOperationException($"Several matches for {currentContext.CurrentToken.GetType()}");

            if (parsed.Length == 1)
            {
                elem = parsed[0];
                return true;
            }

            elem = default;
            return false;
        }

        private MarkdownElement GetParsedOrNull(IMarkdownElementFactory factory, MarkdownElementContext context) =>
            factory.TryCreate(context, out var parsed)
                ? parsed
                : null;
    }
}