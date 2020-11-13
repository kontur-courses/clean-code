using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Formatters
{
    public abstract class BlockFormatter
    {
        protected Dictionary<Style, Func<IEnumerable<string>, IEnumerable<string>>> Wrappers;
        public abstract IEnumerable<string> Format(Style style, IEnumerable<string> words);

        protected static Func<IEnumerable<string>, IEnumerable<string>> Wrap(string open, string close)
        {
            return words => Wrap(open, words, close);
        }

        private static IEnumerable<string> Wrap(string open, IEnumerable<string> words, string close)
        {
            yield return open;
            foreach (var word in words)
                yield return word;
            yield return close;
        }
    }
}