using Markdown.Enums;

namespace Markdown.Tokens;

public class TextToken(int position, string value) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.Text;
    public override string Value => value;
}