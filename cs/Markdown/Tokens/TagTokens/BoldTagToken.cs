using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class BoldTagToken : TagToken
{
    public BoldTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public override string Content => MarkdownTagContentConstants.Bold;
    
    public override int PositionInTokens { get; }

    public static bool IsBoldTagToken(string line, int position)
    {
        if (line.Length - position < MarkdownTagContentConstants.Bold.Length)
            return false;

        return line.Substring(position, MarkdownTagContentConstants.Bold.Length) == MarkdownTagContentConstants.Bold;
    }
}