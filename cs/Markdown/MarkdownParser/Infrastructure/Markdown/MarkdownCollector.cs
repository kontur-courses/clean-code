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

        public bool TryCollectUntil(MarkdownElementContext currentContext, Predicate<Token> predicate,
            out int matchedTokenIndex, out ICollection<Token> collectedTokens)
        {
            collectedTokens = new List<Token>();
            for (var i = 0; i < currentContext.Tokens.Length; i++)
            {
                var token = currentContext.Tokens[i];
                if (predicate.Invoke(token))
                {
                    matchedTokenIndex = i;
                    return true;
                }

                collectedTokens.Add(token);
            }

            matchedTokenIndex = -1;
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