using System.Collections.Generic;

namespace Markdown.Readers
{
    class EmReader : TagReader
        
    {
        public static  IEnumerable<IReader> readers;
        public static  IEnumerable<TagReader> skippedReaders;

        public EmReader() : base("_", "<em>", readers, skippedReaders)
        {
        }
    }
}
