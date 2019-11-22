namespace Markdown.IntermediateState
{
    public enum TagType
    {
        H1 = 1,
        H2 = 2,
        H3 = 3,
        H4 = 4,
        H5 = 5,
        H6 = 6,
        Italic,
        Bold,
        Paragraph,
        Image,
        Raw,
        Link,
        NoneTag, // Leaf of a tree, that doesn't contain any tags
                 // For example <p>abc</p> tree will be like: Paragraph -> NoneTag
        All // Needed if in current tag ANY inner tag need be ignored
            // For example for ```RAW``` tag
    }
}
