using System;
using System.Collections.Generic;

namespace Markdown.Infrastructure.Formatters
{
    public class Wrapper : IWrapper
    {
        public Func<IEnumerable<string>, IEnumerable<string>> Wrap(string open, string close)
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