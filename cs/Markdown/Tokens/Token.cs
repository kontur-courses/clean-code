using System.Text;
using Markdown.Tags;

namespace Markdown.Tokens;

public abstract class Token
{
    public abstract string Str { get; }
    protected abstract Tag Tag { get; }
    protected List<Token> Inner { get; } = new();

    public void AddInner(IEnumerable<Token> tokens)
    {
        Inner.AddRange(tokens);
    }

    public string GetBody()
    {
        return Str.Substring(Tag.MdOpen.Length,
            Str.Length - Tag.MdOpen.Length - Tag.MdClose.Length);
    }

    public abstract string MdString();
    
    protected string MdStringTemplate(IEnumerable<Token> enumerable, Action<StringBuilder> action)
    {
        var replacedStr = new StringBuilder(Str);

        replacedStr.Remove(0, Tag.MdOpen.Length);
        replacedStr.Insert(0, Tag.HtmlOpen);

        action(replacedStr);

        foreach (var token in enumerable)
            replacedStr = replacedStr.Replace(token.Str, token.MdString());

        return replacedStr.ToString();
    }

    public static bool TokenEquals(Token token1, Token token2)
    {
        return token1.Str == token2.Str && token1.Tag.GetType() == token2.Tag.GetType();
    }
}