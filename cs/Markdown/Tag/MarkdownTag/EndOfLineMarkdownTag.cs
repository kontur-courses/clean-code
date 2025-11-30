namespace Markdown;

public class EndOfLineMarkdownTag() : MarkdownTag(TagType.EndOfLine, Environment.NewLine, false)
{
    public override bool IsStartOfTag(string text, int position, bool isSameTagAlreadyOpen)
    {
        if (TagText.Length + position > text.Length) return false;
        
        if (text.AsSpan(position, TagText.Length).Equals(TagText, StringComparison.Ordinal)) return true;
        return false;
    }
}