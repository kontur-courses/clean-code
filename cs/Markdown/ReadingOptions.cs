using System.Collections.Generic;
using Markdown.Readers;

namespace Markdown
{
    public class ReadingOptions
    {
        public List<IReader> AllowedReaders { get; }
        public Dictionary<IReader, HashSet<IReader>> MutedInnerReaders { get; }

        public ReadingOptions(List<IReader> allowedReaders, Dictionary<IReader, HashSet<IReader>> mutedInnerReaders)
        {
            AllowedReaders = allowedReaders;
            MutedInnerReaders = mutedInnerReaders;
        }

        public List<IReader> GetAvailableInnerReadersFor(IReader reader)
        {
            var res = new List<IReader>();
            if (!MutedInnerReaders.TryGetValue(reader, out var mutedReaders) || mutedReaders.Count == 0)
                return AllowedReaders;

            foreach (var innerReader in AllowedReaders)
                if (!mutedReaders.Contains(innerReader))
                    res.Add(innerReader);

            return res;
        }

        public ReadingOptions UpdateAllowedReaders(List<IReader> allowedReaders)
        {
            return new ReadingOptions(allowedReaders, this.MutedInnerReaders);
        }
    }
}