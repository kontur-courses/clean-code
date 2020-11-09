using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser.Infrastructure
{
    /// <summary>
    /// Воркер, который собирает всякие штуки по токенам
    /// </summary>
    public class MarkdownCollector
    {
        private readonly ICollection<IMarkdownElementProvider> providers = new List<IMarkdownElementProvider>();

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

        public IEnumerable<MarkdownElement> ParseElementsFrom(params Token[] tokens)
        {
            //TODO class Subarray
            for (var i = 0; i < tokens.Length;)
            {
                var token = tokens[i];
                var currentContext = new MarkdownElementContext(token, tokens.Skip(i + 1));
                if (token is TokenText || !TryCreateElementFrom(currentContext, out var element))
                    element = new MarkdownText(token);
                yield return element;
                i += element.Tokens.Length;
            }
        }

        public void RegisterProvider(IMarkdownElementProvider provider) => providers.Add(provider);

        private bool TryCreateElementFrom(MarkdownElementContext currentContext, out MarkdownElement elem)
        {
            var parsed = providers.Select(p => GetParsedOrNull(p, currentContext))
                .Where(x => x != null)
                .ToArray();
            
            if(parsed.Length > 1)
                throw new InvalidOperationException($"Several matches for {currentContext.CurrentToken.GetType()}");

            if (parsed.Length == 1)
            {
                elem = parsed[0];
                return true;
            }

            elem = default;
            return false;
        }

        private MarkdownElement GetParsedOrNull(IMarkdownElementProvider provider, MarkdownElementContext context) =>
            provider.TryParse(context, out var parsed) 
                ? parsed 
                : null;
    }
}