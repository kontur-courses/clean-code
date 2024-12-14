using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class ItalicsTagToken : TagToken
{
    public ItalicsTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public override string Content => MarkdownTagContentConstants.Italics;
    
    public override int PositionInTokens { get; }

    public static bool IsItalicsTagToken(string line, int position)
    {
        return line[position].ToString() == MarkdownTagContentConstants.Italics;
    }
}