using System.Collections.Generic;

namespace Markdown.Readers
{
    public class StrongReader : TagReader
    {
        public static  IEnumerable<IReader> readers;
        public static  IEnumerable<TagReader> skippedReaders;

        public StrongReader(string mdTag, (string, string) tagShell) : base(mdTag, tagShell, readers, skippedReaders)
        {
        }
    }
}
