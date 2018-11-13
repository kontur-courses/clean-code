
namespace Markdown.Readers
{
    class SetupReaders
    {
        public static void Setup()
        {
            EmReader.readers = new IReader[] {new SlashReader(), new CharReader()};
            EmReader.skippedReaders = new TagReader[] {new StrongReader()};

            StrongReader.readers = new IReader[] {new SlashReader(), new EmReader(), new CharReader()};
            StrongReader.skippedReaders = new TagReader[0];
        }
    }
}
