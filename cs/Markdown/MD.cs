using Markdown.Contracts;
using Markdown.Tokens;

namespace Markdown;

public class MD
{
    private readonly Dictionary<string, Func<ITag>> tagFactory = new();
    private readonly Tokenizer tokenizer;

    public MD()
    {
        tokenizer = new Tokenizer(this);
    }

    public List<Token> Render(string text)
    {
        var tokens = tokenizer.CollectTokens(text);

        return tokens;
        throw new NotImplementedException();
    }

    public void AddFactoryFor(string mark, Func<ITag> creationMethod)
    {
        if (mark == null)
            throw new NullReferenceException("Tag mark can't be null");

        tagFactory.Add(mark, creationMethod);
    }

    public ITag? GetInstanceViaMark(string mark)
    {
        foreach (var tagMark in tagFactory.Keys)
            if (mark.StartsWith(tagMark))
                return tagFactory[tagMark]();

        return null;
    }
}