using Markdown.Enums;

namespace Markdown.Tokens;

public class SpaceToken(int position) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.Space;
    public override string Value => " ";
}