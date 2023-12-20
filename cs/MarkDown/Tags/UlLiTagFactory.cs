using MarkDown.Enums;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class UlLiTagFactory : TagFactory
{
    public UlLiTagFactory(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override TagName TagName => TagName.UlLi;
    public override string HtmlOpen => "<li>";
    public override string HtmlClose => "</li>";
    public override string MarkDownOpen => "+ ";
    public override string MarkDownClose => System.Environment.NewLine;
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

        return text[position - 1].ToString().Equals(System.Environment.NewLine);
    }

    public override bool IsClosePosition(string text, int position)
    {
        // if (position == 0)
        //     return false;
        //
        // if (!text[position - 1].ToString().Equals(System.Environment.NewLine))
        //     return false;
        //
        // if (position + 2 >= text.Length)
        //     return true;
        //
        // return !LiOpens.Any(e => e.Equals(text.Substring(position, 2)));
        return text[position].ToString().Equals(System.Environment.NewLine);
    }
}