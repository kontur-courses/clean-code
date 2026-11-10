using System.Text;

namespace Markdown.Tokens;

internal class HeaderToken(string value, int start)
    : Token(value, start, openingHtml: "<h1>", closingHtml: "</h1>");