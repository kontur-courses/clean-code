using System.Text;

namespace Markdown.Tokens;

public class UnderscoreToken(string value, int start)
    : Token(value, start);