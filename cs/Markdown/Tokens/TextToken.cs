using System.Text;

namespace Markdown.Tokens;

public class TextToken(string value, int start)
{
    protected string Value { get; } = value;

    public int Start { get; } = start;

    public int End => Start + Value.Length;

    public virtual StringBuilder WriteHtml()
        => new(Value);
}
