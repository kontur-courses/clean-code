using Markdown.Contracts;

namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public TagStatus Status { get; set; }
    public ContextInfo Context { get; set; }
    public TagType Type => TagType.Header;
    public TagInfo Info => new("# ", "<h1>", "</h1>");

    public void ChangeStatusIfBroken()
    {
        for (var i = 0; i < Context.Position; i++)
        {
            if (char.IsWhiteSpace(Context.Text[i])) 
                continue;
            
            Status = TagStatus.Broken;
            return;
        }
    }
}