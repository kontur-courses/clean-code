namespace Markdown.TagReaders;

public class TagReadersProvider
{
    private readonly Dictionary<char, ITagReader> readers = new();

    private readonly HashSet<char> tagsSymbols = new();

    public TagReadersProvider(IEnumerable<ITagReader> readers)
    {
        foreach (var reader in readers)
        {
            this.readers[reader.Opener] = reader;
            foreach (var tagSymbol in reader.TagSymbols)
            {
                tagsSymbols.Add(tagSymbol);
            }
        }
    }

    public bool TryGetReader(char c, out ITagReader tagReader)
    {
        if (!readers.TryGetValue(c, out var reader))
        {
            tagReader = null;
            return false;
        }

        tagReader = reader;
        return true;
    }

    public bool IsTagSymbol(char c) => tagsSymbols.Contains(c);

    public bool IsTagOpener(char c) => readers.ContainsKey(c);
}