using System.Text;

namespace Markdown.Tokens;

internal class DoubleUnderscoreToken(string value, int start)
    : Token(value, start, openingHtml: "<strong>", closingHtml: "</strong>");