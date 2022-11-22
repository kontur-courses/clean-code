using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions;

public interface IElementsNestingRules
{
    public bool CanContainNested(IElement parent, IElement nested);
    public bool CanContainNested(IElement parent, Type nested);
    public bool CanContainNested(Type parent, IElement nested);
    public bool CanContainNested(Type parent, Type nested);
}