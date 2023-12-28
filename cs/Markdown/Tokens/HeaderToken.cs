using System.Text;
using Markdown.Tags;

namespace Markdown.Tokens;

public class HeaderToken(string str) : IToken
{
    public string Str { get; } = str;
    public List<IToken> Inner { get; } = new();
    public ITag Tag { get; } = new HeaderTag();
    
    public string MdString()
    {
        var replacedStr = new StringBuilder(Str);

        replacedStr.Remove(0, Tag.MdOpen.Length);
        replacedStr.Insert(0, Tag.HtmlOpen);

        if (replacedStr[^Tag.MdClose.Length].ToString() == Tag.MdClose)
            replacedStr.Insert(replacedStr.Length - Tag.MdClose.Length, Tag.HtmlClose);
        else
            replacedStr.Insert(replacedStr.Length, Tag.HtmlClose);
        
        foreach (var token in Inner)
            replacedStr = replacedStr.Replace(token.Str, token.MdString());

        return replacedStr.ToString();
    }
}