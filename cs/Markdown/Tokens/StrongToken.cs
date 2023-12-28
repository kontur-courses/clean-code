using System.Text;
using Markdown.Tags;

namespace Markdown.Tokens;

public class StrongToken(string str) : IToken
{
    public string Str { get; } = str;
    public List<IToken> Inner { get; } = new();
    public ITag Tag { get; } = new StrongTag();
    
    public string MdString()
    {
        var replacedStr = new StringBuilder(Str);

        replacedStr.Remove(0, Tag.MdOpen.Length);
        replacedStr.Insert(0, Tag.HtmlOpen);

        var tmpLen = replacedStr.Length;
        replacedStr.Remove(tmpLen - Tag.MdClose.Length, Tag.MdClose.Length);
        replacedStr.Insert(tmpLen - Tag.MdClose.Length, Tag.HtmlClose);

        foreach (var token in Inner)
            replacedStr = replacedStr.Replace(token.Str, token.MdString());

        return replacedStr.ToString();
    }
}