using Markdown.Tags;

namespace Markdown.Tokens;

public class EmToken(string str) : Token
{
    public override string Str { get; } = str;
    protected override Tag Tag { get; } = new EmTag();

    public override string MdString()
    {
        return MdStringTemplate(
            Inner.Where(token => token is not StrongToken),
            replacedStr =>
            {
                var tmpLen = replacedStr.Length;
                replacedStr.Remove(tmpLen - Tag.MdClose.Length, Tag.MdClose.Length);
                replacedStr.Insert(tmpLen - Tag.MdClose.Length, Tag.HtmlClose);
            });
    }
}