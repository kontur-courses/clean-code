using Markdown.Tags;

namespace Markdown.Tokens;

public class StrongToken(string str) : Token
{
    public override string Str { get; } = str;
    protected override Tag Tag { get; } = new StrongTag();

    public override string MdString()
    {
        return MdStringTemplate(
            Inner,
            replacedStr =>
            {
                var tmpLen = replacedStr.Length;
                replacedStr.Remove(tmpLen - Tag.MdClose.Length, Tag.MdClose.Length);
                replacedStr.Insert(tmpLen - Tag.MdClose.Length, Tag.HtmlClose);
            });
    }
}