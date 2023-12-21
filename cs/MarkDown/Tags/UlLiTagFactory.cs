using MarkDown.Enums;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class UlLiTagFactory : TagFactory
{
    public override TagName TagName => TagName.UlLi;
    public override string HtmlOpen => "<li>";
    public override string HtmlClose => "</li>";
    public override string MarkDownOpen => "+ ";
    public override string MarkDownClose => Environment.NewLine;
    private string[] LiOpens => new[] {"+ ", "- ", "* "};
    
    public override TagContext CreateContext(string mdText, int startIndex, TagContext parentContext, bool isScreened)
    {
        return new UlLiContext(startIndex, parentContext, this, isScreened);
    }

    public override bool CanCreateContext(string text, int position, TagContext parentContext)
    {
        if (parentContext is not UlContext)
            return false;
        
        if (position + 2 >= text.Length)
            return false;

        if (!LiOpens.Any(e => text.Substring(position, 2).Equals(e)))
            return false;

        if (position == 0)
            return true;

        return text[position - 1].ToString().Equals(Environment.NewLine);
    }

    public override bool IsClosePosition(string text, int position)
    {
        return text[position].ToString().Equals(Environment.NewLine);
    }
}