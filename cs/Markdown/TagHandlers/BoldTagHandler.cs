namespace Markdown.TagHandlers;

public class BoldTagHandler: PairedTagsHandler, ITagHandler
{
    public BoldTagHandler() : base("__", "<strong>")
    {
        NestedTagHandlers = new ITagHandler[]
        {
            new ItalicTagHandler(),
            new LinkTagHandler(),
        };
    }
    
    protected override ITagHandler[] NestedTagHandlers { get; }
}