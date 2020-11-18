using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown
{
    /// <summary>
    /// Воркер, который собирает всякие штуки по токенам
    /// </summary>
    public class MarkdownCollector
    {
        private readonly ICollection<IMdElementFactory> providers;

        public MarkdownCollector(IEnumerable<IMdElementFactory> providers)
        {
            this.providers = providers.ToArray();
            foreach (var dependentProvider in this.providers.OfType<IMarkdownCollectorDependent>())
                dependentProvider.SetCollector(this);
        }

        public IEnumerable<MarkdownElement> CreateElementsFrom(params Token[] tokens)
        {
            for (var i = 0; i < tokens.Length;)
            {
                var token = tokens[i];
                if (token is TextToken || !TryCreateElementFrom(token, tokens.Skip(i + 1).ToArray(), out var element))
                    element = new MarkdownText(token);
                yield return element;
                i += element.Tokens.Length;
            }
        }

        private bool TryCreateElementFrom(Token current, Token[] next, out MarkdownElement elem)
        {
            var parsed = providers.Where(p => p.CanCreate(current)).ToArray();

            if (parsed.Length > 1)
                throw new InvalidOperationException($"Several matches for {current.GetType()}");

            if (parsed.Length == 1)
            {
                elem = parsed[0].Create(current, next);
                return true;
            }

            elem = default;
            return false;
        }
    }
}