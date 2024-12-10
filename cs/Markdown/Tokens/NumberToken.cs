using Markdown.Enums;

namespace Markdown.Tokens;

public class NumberToken(int position, string value) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.Number;
    public override string Value => value;
}