using System.Text;

namespace Markdown.Tokens;

public sealed class LinkToken(string raw, int start, string url)
    : Token(raw, start)
{
    public string Url => url;
}