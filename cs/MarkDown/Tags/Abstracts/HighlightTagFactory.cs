using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;

namespace MarkDown.Tags.Abstracts;

public abstract class HighlightTagFactory : TagFactory
{
    protected HighlightTagFactory(MarkDownEnvironment environment) : base(environment)
    {
    }
    
    public override TagContext CreateContext(string mdText, int startIndex, TagContext parentContext, bool isScreened)
    {
        var isInWord = startIndex > 0 && mdText[startIndex - 1] != ' ';

        return new HighlightContext(startIndex, isInWord, parentContext, this, isScreened);
    }
    
    public override bool CanCreateContext(string text, int position)
    {
        if (position + MarkDownOpen.Length > text.Length)
            return false;

        if (!text.Substring(position, MarkDownOpen.Length).Equals(MarkDownOpen))
            return false;
        
        if (position > 0)
            if (char.IsDigit(text[position - 1]) || text[position - 1] == '_')
                return false;
        
        if (position + MarkDownOpen.Length >= text.Length)
            return true;

        var nextPos = position + MarkDownOpen.Length;
        
        return !char.IsDigit(text[nextPos]) && text[nextPos] != ' ' && text[nextPos] != '_';
    }

    public override bool IsClosePosition(string text, int position)
    {
        if (position + MarkDownClose.Length > text.Length)
            return false;

        if (!text.Substring(position, MarkDownClose.Length).Equals(MarkDownClose))
            return false;
        
        if (position > 0)
            if (char.IsDigit(text[position - 1]) || text[position - 1] == ' ' || text[position - 1] == '_')
                return false;
        
        if (position + MarkDownClose.Length >= text.Length)
            return true;
        
        var nextPos = position + MarkDownOpen.Length;
        
        return !char.IsDigit(text[nextPos]) && text[nextPos] != '_';
    }
}