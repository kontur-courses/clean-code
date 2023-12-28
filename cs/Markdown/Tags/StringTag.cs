using Markdown.Tokens;

namespace Markdown.Tags;

public class StringTag : ITag
{
    public string MdOpen => "";
    public string MdClose => "";
    public string HtmlOpen => "";
    public string HtmlClose => "";

    public IToken? TryFindToken(string text, int idx)
    {
        return null;
    }

    public bool IsOpenTag(string text, int idx)
    {
        return true;
    }

    public bool IsCloseTag(string text, int idx)
    {
        return true;
    }
}