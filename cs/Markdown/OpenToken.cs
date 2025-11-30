namespace Markdown;

public class OpenToken(MarkdownTag openTag, int textStartPosition)
{
    public MarkdownTag OpenTag = openTag;
    public int TextStartPosition = textStartPosition;
    public readonly List<Token> NestedTokens = [];
}