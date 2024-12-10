using Markdown.Enums;

namespace Markdown.Tokens;

public class NewLineToken(int position) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.NewLine;
    public override string Value => "\n";
}