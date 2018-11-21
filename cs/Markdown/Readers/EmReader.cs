using System.Collections.Generic;

namespace Markdown.Readers
{
    public class EmReader : TagReader

    {
        public static IEnumerable<IReader> Readers;
        public static IEnumerable<TagReader> SkippedReaders;

        public EmReader() : base("_", Readers, SkippedReaders)
        {
        }
    }
}