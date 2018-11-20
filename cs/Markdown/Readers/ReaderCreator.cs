
using System.Collections.Generic;

namespace Markdown.Readers
{
    public static class ReaderCreator
    {
        public static List<IReader> Create()
        {
            IReader slashReader = new SlashReader();
            IReader charReader = new CharReader();

            EmReader.Readers = new[] { slashReader, charReader };
            EmReader.SkippedReaders = new TagReader[] { new StrongReader("__") };

            TagReader emTagReader = new EmReader("_");

            StrongReader.Readers = new[] { slashReader, emTagReader, charReader };
            StrongReader.SkippedReaders = new TagReader[0];
            TagReader strongTagReader = new StrongReader("__");

            return new List<IReader> { slashReader, strongTagReader, emTagReader, charReader };
        }
    }
}
