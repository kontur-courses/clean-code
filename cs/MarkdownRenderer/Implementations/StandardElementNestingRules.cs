using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations;

public class StandardElementsNestingRules : IElementsNestingRules
{
    private readonly IReadOnlyDictionary<Type, IReadOnlySet<Type>> _rules = new Dictionary<Type, IReadOnlySet<Type>>
    {
        {
            typeof(PlainTextElement), new HashSet<Type>()
        },
        {
            typeof(LinkElement), new HashSet<Type>()
        },
        {
            typeof(EscapeSequenceElement), new HashSet<Type>
            {
                typeof(PlainTextElement)
            }
        },
        {
            typeof(ItalicElement), new HashSet<Type>
            {
                typeof(PlainTextElement), typeof(LinkElement), typeof(EscapeSequenceElement)
            }
        },
        {
            typeof(StrongElement), new HashSet<Type>
            {
                typeof(PlainTextElement), typeof(LinkElement),
                typeof(EscapeSequenceElement), typeof(ItalicElement)
            }
        },
        {
            typeof(ParagraphElement), new HashSet<Type>
            {
                typeof(PlainTextElement), typeof(LinkElement),
                typeof(EscapeSequenceElement), typeof(ItalicElement), typeof(StrongElement)
            }
        },
        {
            typeof(HeaderElement), new HashSet<Type>
            {
                typeof(PlainTextElement), typeof(LinkElement),
                typeof(EscapeSequenceElement), typeof(ItalicElement), typeof(StrongElement)
            }
        }
    };

    public bool CanContainNested(IElement parent, IElement nested) =>
        CanContainNested(parent.GetType(), nested.GetType());

    public bool CanContainNested(IElement parent, Type nested) =>
        CanContainNested(parent.GetType(), nested);

    public bool CanContainNested(Type parent, IElement nested) =>
        CanContainNested(parent, nested.GetType());

    public bool CanContainNested(Type parent, Type nested)
    {
        if (!_rules.TryGetValue(parent, out var nesting))
            throw new ArgumentException($"No rules provided for element of type: {parent.Name}");
        return nesting.Contains(nested);
    }
}