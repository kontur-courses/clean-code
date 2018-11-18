
using System.Collections.Generic;

namespace Markdown.Readers
{
    public class CreateReaders
    {
        public static List<IReader> Create()
        {
            IReader slashReader = new SlashReader();
            IReader charReader = new CharReader();

            EmReader.readers = new[] { slashReader, charReader };
            EmReader.skippedReaders = new TagReader[] { new StrongReader("__", Translator.TranslateDictionary["__"]) };

            TagReader emTagReader = new EmReader("_", Translator.TranslateDictionary["_"]);

            StrongReader.readers = new[] { slashReader, emTagReader, charReader };
            StrongReader.skippedReaders = new TagReader[0];
            TagReader strongTagReader = new StrongReader("__", Translator.TranslateDictionary["__"]);

            return new List<IReader> { slashReader, strongTagReader, emTagReader, charReader };
        }
    }
}
