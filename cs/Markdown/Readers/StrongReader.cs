using System.Collections.Generic;

namespace Markdown.Readers
{
    class StrongReader : TagReader
    {
        public static  IEnumerable<IReader> readers;
        public static  IEnumerable<TagReader> skippedReaders;

        public StrongReader() : base("__", "<strong>", readers, skippedReaders)
        {
        }
    }
}
