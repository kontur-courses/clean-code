using Markdown.Tokens;

namespace Markdown.Tags;

public interface ITag
{
    public string MdOpen { get; }
    public string MdClose { get; }
    public string HtmlOpen { get; }
    public string HtmlClose { get; }

    public IToken? TryFindToken(string text, int idx);

    protected bool IsOpenTag(string text, int idx);

    protected bool IsCloseTag(string text, int idx);
}