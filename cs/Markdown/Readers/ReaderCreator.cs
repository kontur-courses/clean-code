
using System.Collections.Generic;

namespace Markdown.Readers
{
    public static class ReaderCreator
    {
        public static List<IReader> Create()
        {
            IReader slashReader = new SlashReader();
            IReader charReader = new CharReader();

            CodeReader.Readers =  new[] { slashReader, charReader };
            CodeReader.SkippedReaders = new List<TagReader> {new StrongReader("__"), new EmReader("_")};
            CodeReader codeReader = new CodeReader("`");
            
            EmReader.Readers = new[] { slashReader, codeReader, charReader};
            EmReader.SkippedReaders = new TagReader[] { new StrongReader("__") };
            EmReader emTagReader = new EmReader("_");

            StrongReader.Readers = new[] { slashReader, codeReader, emTagReader, charReader };
            StrongReader.SkippedReaders = new TagReader[0];
            StrongReader strongTagReader = new StrongReader("__");
            
            
            return new List<IReader> { slashReader, codeReader, strongTagReader, emTagReader, charReader };
        }
    }
}
