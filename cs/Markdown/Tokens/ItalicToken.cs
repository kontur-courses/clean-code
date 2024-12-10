using Markdown.Enums;

namespace Markdown.Tokens;

public class ItalicToken(int position) : Token(position)
{
    public override MarkdownTokenName Name => MarkdownTokenName.Italic;
    public override string Value => "_";
}