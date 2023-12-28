using Markdown.Tokens;

namespace Markdown.Tags;

public class StringTag : Tag
{
    public override string MdOpen => "";
    public override string MdClose => "";
    public override string HtmlOpen => "";
    public override string HtmlClose => "";

    public override Token? TryFindToken(string text, int idx)
    {
        return null;
    }

    protected override bool IsOpenTag(string text, int idx)
    {
        return true;
    }

    protected override bool IsCloseTag(string text, int idx)
    {
        return true;
    }
}