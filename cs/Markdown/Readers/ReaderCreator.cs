
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
            CodeReader.SkippedReaders = new List<TagReader> {new StrongReader(), new EmReader()};
            CodeReader codeReader = new CodeReader();
            
            
            EmReader.Readers = new[] { slashReader, codeReader, charReader};
            EmReader.SkippedReaders = new TagReader[] { new StrongReader() };
            EmReader emTagReader = new EmReader();
            
            StrongReader.Readers = new[] { slashReader, codeReader, emTagReader, charReader };
            StrongReader.SkippedReaders = new TagReader[0];
            
            
            return new List<IReader> { new SlashReader(),
                new CodeReader(), new StrongReader(), new EmReader(), new CharReader() };
        }
    }
}
