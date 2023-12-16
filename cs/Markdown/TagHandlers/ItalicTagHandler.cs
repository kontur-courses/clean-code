using NUnit.Framework.Internal;

namespace Markdown.TagHandlers;

public class ItalicTagHandler: PairedTagsHandler, ITagHandler
{
    public ItalicTagHandler() : base("_", "<em>")
    {
        NestedTagHandlers = new ITagHandler[]
        {
            new LinkTagHandler()
        };
    }

    protected override ITagHandler[] NestedTagHandlers { get; }
}