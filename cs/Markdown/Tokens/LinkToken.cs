using System.Text;

namespace Markdown.Tokens;

internal sealed class LinkToken(string raw, int start, string url)
    : Token(raw, start, openingHtml: string.Format($"<a href=\"{url}\">"), closingHtml: "</a>");