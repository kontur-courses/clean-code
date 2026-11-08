namespace Markdown.TagReaders;

public class TagReadersProvider
{
    private readonly Dictionary<char, ITagReader> readers = new();

    private readonly TextReader textReader;

    private HashSet<char> tagsSymbols = new();

    public TagReadersProvider(IEnumerable<ITagReader> readers)
    {
        foreach (var reader in readers)
        {
            if (reader is TextReader textReader)
            {
                this.textReader = textReader;
            }
            else
            {
                this.readers[reader.Opener] = reader;
                foreach (var tagSymbol in reader.TagSymbols)
                {
                    tagsSymbols.Add(tagSymbol);
                }
            }
        }
    }

    public ITagReader GetReader(char c)
    {
        if (!readers.TryGetValue(c, out var reader))
        {
            return textReader;
        }
        return reader;
    }

    public bool IsTagSymbol(char c) => tagsSymbols.Contains(c);
}
