using Markdown.Tags;

namespace Markdown.Tokens;

public class HeaderToken(string str) : Token
{
    public override string Str { get; } = str;
    protected override Tag Tag { get; } = new HeaderTag();

    public override string MdString()
    {
        return MdStringTemplate(
            Inner,
            replacedStr =>
                replacedStr.Insert(
                    replacedStr[^Tag.MdClose.Length].ToString() == Tag.MdClose
                        ? replacedStr.Length - Tag.MdClose.Length
                        : replacedStr.Length, Tag.HtmlClose)
        );
    }
}