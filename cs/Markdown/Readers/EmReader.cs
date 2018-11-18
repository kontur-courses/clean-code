using System.Collections.Generic;

namespace Markdown.Readers
{
    public class EmReader : TagReader
        
    {
        public static  IEnumerable<IReader> readers;
        public static  IEnumerable<TagReader> skippedReaders;

        public EmReader(string mdTag, (string, string) tagShell) : base(mdTag, tagShell, readers, skippedReaders)
        {
        }
    }
}
