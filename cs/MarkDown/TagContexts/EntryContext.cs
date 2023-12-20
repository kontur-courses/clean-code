using System.Text;
using MarkDown.Tags;

namespace MarkDown.TagContexts;

public class EntryContext : TagContext
{
    public EntryContext(int startIndex, TagContext? context, Tag creator) : base(startIndex, context, creator)
    {
    }
    
    public EntryContext(Tag creator) : base(0, null, creator)
    {
    }

    protected override void HandleSymbolItself(char symbol)
    {
    }

    public override (int start, int end) ConvertToHtml(string text, StringBuilder sb, MarkDownEnvironment environment)
    {
        var start = StartIndex;
        
        foreach (var context in InnerContexts
                     .Where(context => context.Closed))
        {
            var (innerStart, innerEnd) = context.ConvertToHtml(text, sb, environment);
            sb.Insert(0, text.AsSpan(start, innerStart - start));
            start = innerEnd;
        }
        
        if (start < text.Length)
            sb.Append(text.AsSpan(start, CloseIndex - start));
        
        return (StartIndex, CloseIndex);
    }

    public override void CloseSingleTags(int closeIndex)
    {
        Closed = true;
        CloseIndex = closeIndex;
    }
}