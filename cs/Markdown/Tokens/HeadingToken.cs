using Markdown.Enums;

namespace Markdown.Tokens;

public class HeadingToken(int position) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.Heading;
    public override string Value => "# ";
}