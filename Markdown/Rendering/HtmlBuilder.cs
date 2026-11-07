using Markdown.Tokenization;
using System.Text;

namespace Markdown.Rendering;

public class HtmlBuilder
{
    private readonly StringBuilder _sb = new();

    public void Append(IEnumerable<Token> tokens)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
        => _sb.ToString();
}
