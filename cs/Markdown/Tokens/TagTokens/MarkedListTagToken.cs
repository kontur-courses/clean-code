using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class MarkedListTagToken : TagToken
{
    public MarkedListTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public override string Content => MarkdownTagContentConstants.MarkedList;
    
    public override int PositionInTokens { get; }
    
    public static bool IsMarkedListTagToken(string line, int position)
    {
        return line[position].ToString() == MarkdownTagContentConstants.MarkedList;
    }
}