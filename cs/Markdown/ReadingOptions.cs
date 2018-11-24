using System.Collections.Generic;
using Markdown.Readers;

namespace Markdown
{
    public class ReadingOptions
    {
        public List<AbstractReader> AllowedReaders { get; }
        private Dictionary<AbstractReader, HashSet<AbstractReader>> AllowedInnerReadersByDefault { get; }

        public ReadingOptions(List<AbstractReader> allowedReaders, Dictionary<AbstractReader, HashSet<AbstractReader>> allowedInnerReaders)
        {
            AllowedReaders = allowedReaders;
            AllowedInnerReadersByDefault = allowedInnerReaders;
        }

        public List<AbstractReader> GetAvailableInnerReadersFor(AbstractReader reader)
        {
            var res = new List<AbstractReader>();
            if (!AllowedInnerReadersByDefault.TryGetValue(reader, out var allowedInnerReaders) || allowedInnerReaders.Count == 0)
                return res;

            foreach (var innerReader in AllowedReaders)
                if (allowedInnerReaders.Contains(innerReader))
                    res.Add(innerReader);

            return res;
        }

        public ReadingOptions WithAllowedReaders(List<AbstractReader> allowedReaders)
        {
            return new ReadingOptions(allowedReaders, this.AllowedInnerReadersByDefault);
        }
    }
}