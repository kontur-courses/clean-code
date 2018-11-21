using System.Collections.Generic;

namespace Markdown.Readers
{
    public class CodeReader : TagReader
    {
        public static IEnumerable<IReader> Readers;
        public static IEnumerable<TagReader> SkippedReaders;

        public CodeReader() : base("`", Readers, SkippedReaders)
        {
        }
    }
}