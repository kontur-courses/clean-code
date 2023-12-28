using Markdown.Tags;

namespace Markdown.Tokens;

public interface IToken
{
    public string Str { get; }
    public List<IToken> Inner { get; }
    public ITag Tag { get; }

    void AddInner(IEnumerable<IToken> tokens)
    {
        Inner.AddRange(tokens);
    }

    string GetBody()
    {
        return Str.Substring(Tag.MdOpen.Length,
            Str.Length - Tag.MdOpen.Length - Tag.MdClose.Length);
    }

    string MdString();

    static bool TokenEquals(IToken token1, IToken token2)
    {
        return token1.Str == token2.Str && token1.Tag.GetType() == token2.Tag.GetType();
    }
}