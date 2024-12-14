using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class HeadingTagToken : TagToken
{
    public HeadingTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public override string Content => MarkdownTagContentConstants.Heading;
    
    public override int PositionInTokens { get; }

    public static bool IsHeadingTagToken(string line, int position)
    {
        return line[position].ToString() == MarkdownTagContentConstants.Heading;
    }
}