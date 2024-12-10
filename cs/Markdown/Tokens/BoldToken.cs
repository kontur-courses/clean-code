using Markdown.Enums;

namespace Markdown.Tokens;

public class BoldToken(int position) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.Bold;
    public override string Value => "__";
}