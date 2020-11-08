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
        private IMarkdownElementProvider defaultProvider;

        public bool TryCollectUntil(MarkdownElementContext currentContext, Predicate<Token> predicate,
            out int matchedTokenIndex, out ICollection<Token> collectedTokens)
        {
            collectedTokens = new List<Token>();
            for (var i = currentContext.CurrentTokenIndex + 1; i < currentContext.AllTokens.Length; i++)
            {
                var token = currentContext.AllTokens[i];
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

        public bool TryParseUntil(MarkdownElementContext currentContext, Predicate<Token> predicate,
            out ICollection<MarkdownElement> parsedElements, out int lastVisitedTokenIndex)
        {
            parsedElements = new List<MarkdownElement>();

            lastVisitedTokenIndex = currentContext.CurrentTokenIndex;
            for (var i = currentContext.CurrentTokenIndex + 1; i < currentContext.AllTokens.Length; i++)
            {
                lastVisitedTokenIndex = i;
                var token = currentContext.CurrentToken;
                if (predicate.Invoke(token))
                    return true;

                var createdElement = CreateElementFrom(new MarkdownElementContext(i, currentContext.AllTokens));
                parsedElements.Add(createdElement);
                i = createdElement.LastTokenIndex;
            }

            return false;
        }

        public void RegisterProvider(IMarkdownElementProvider provider) => providers.Add(provider);
        public void SetDefaultProvider(IMarkdownElementProvider provider) => defaultProvider = provider;

        private MarkdownElement CreateElementFrom(MarkdownElementContext currentContext)
        {
            var markdownElements = providers.Select(mp => new
                {
                    IsSuccessful = mp.TryParse(currentContext, out var elem),
                    Element = elem
                })
                .Where(x => x.IsSuccessful)
                .Select(x => x.Element)
                .ToArray();

            return markdownElements.Length switch
            {
                1 => markdownElements[0],

                0 when defaultProvider != null && defaultProvider.TryParse(currentContext, out var parsed)
                    => parsed,

                0 => throw new InvalidOperationException("Cannot create markdown from token " +
                                                         $"{currentContext.CurrentToken.GetType()}"),

                _ => throw new InvalidOperationException("Multiple markdown parser matched for " +
                                                         $"{currentContext.CurrentToken.GetType()}")
            };
        }
    }
}