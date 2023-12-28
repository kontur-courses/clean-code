using Markdown.Tags;

namespace Markdown.Tokens;

public class LinkToken(string str) : Token
{
    public override string Str { get; } = str;
    protected override Tag Tag { get; } = new LinkTag();

    public override string MdString()
    {
        var startIdxName = Str.IndexOf('[');
        var name = Str.Substring(startIdxName + 1, Str.IndexOf(']') - startIdxName - 1);

        var startIdxLink = Str.IndexOf('(');
        var inBracers = Str.Substring(startIdxLink + 1, Str.IndexOf(')') - startIdxLink - 1);

        var splitted = inBracers.Split(" ");

        return splitted.Length == 2
            ? $"{Tag.HtmlOpen}{splitted[0]}\" title={splitted[1]}>{name}{Tag.HtmlClose}"
            : $"{Tag.HtmlOpen}{splitted[0]}\">{name}{Tag.HtmlClose}";
    }
}