using System.Text;
using MarkDown.Tags;

namespace MarkDown.TagContexts;

public class HeaderContext : TagContext
{
    public HeaderContext(int startIndex, TagContext tagContext, Tag creator) : base(startIndex, tagContext, creator)
    {
    }

    protected override void HandleSymbolItself(char symbol)
    {
    }

    public override (int start, int end) ConvertToHtml(string text, StringBuilder sb, MarkDownEnvironment environment)
    {
        var start = StartIndex + Creator.MarkDownOpen.Length;
        sb.Append(Creator.HtmlOpen);
        
        foreach (var context in InnerContexts
                     .Where(context => context.Closed))
        {
            var (innerStart, innerEnd) = context.ConvertToHtml(text, sb, environment);
            sb.Insert(0, text.AsSpan(start, innerStart - start));
            start = innerEnd;
        }
        
        sb.Append(text.AsSpan(start, CloseIndex - start));

        sb.Append(Creator.HtmlClose);
        
        return (StartIndex, CloseIndex + Creator.MarkDownClose.Length);
    }

    public override void CloseSingleTags(int closeIndex)
    {
        parent.CloseSingleTags(closeIndex);
        
        if (Closed)
            return;
        
        CloseIndex = closeIndex;
        Closed = true;
    }
}