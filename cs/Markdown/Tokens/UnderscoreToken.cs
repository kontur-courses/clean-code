using System.Text;

namespace Markdown.Tokens;

internal class UnderscoreToken(string value, int start)
    : Token(value, start, openingHtml: "<em>", closingHtml: "</em>");