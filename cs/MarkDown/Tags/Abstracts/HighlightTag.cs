namespace MarkDown.Tags.Abstracts;

public abstract class HighlightTag : Tag
{
    protected HighlightTag(MarkDownEnvironment environment) : base(environment)
    {
    }
    
    public override bool CanCreateContext(string text, int position)
    {
        if (position + MarkDownOpen.Length > text.Length)
            return false;

        if (!text.Substring(position, MarkDownOpen.Length).Equals(MarkDownOpen))
            return false;
        
        if (position > 0)
            if (char.IsDigit(text[position - 1]))
                return false;
        
        if (position + MarkDownOpen.Length + 1 > text.Length)
            return true;

        var nextPos = position + MarkDownOpen.Length;
        
        return !char.IsDigit(text[nextPos]) && text[nextPos] != ' ';
    }

    public override bool IsClosePosition(string text, int position)
    {
        if (position + MarkDownClose.Length > text.Length)
            return false;

        if (!text.Substring(position, MarkDownClose.Length).Equals(MarkDownClose))
            return false;
        
        if (position > 0)
            if (char.IsDigit(text[position - 1]) || text[position - 1] == ' ')
                return false;
        
        if (position + MarkDownClose.Length + 1 > text.Length)
            return true;
        
        return !char.IsDigit(text[position + MarkDownClose.Length]);
    }
}