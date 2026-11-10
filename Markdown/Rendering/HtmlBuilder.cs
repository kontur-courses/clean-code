using Markdown.Tokenization.Tokens;
using System.Text;

namespace Markdown.Rendering;

public class HtmlBuilder
{
    private readonly StringBuilder _sb = new();

    public void Append(ResolvedTokens tokens)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
        => _sb.ToString();
}
