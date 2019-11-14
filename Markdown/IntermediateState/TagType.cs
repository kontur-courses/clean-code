namespace Markdown.IntermediateState
{
    enum TagType
    {
        Italic,
        Bold,
        H1,
        H2,
        H3,
        H4,
        H5,
        H6,
        Paragraph,
        Image,
        Raw,
        NoneTag, // Leaf of a tree, that doesn't contain any tags
                 // For example <p>abc</p> tree will be like: Paragraph -> NoneTag
        All // Needed if in current tag ANY inner tag need be ignored
            // For example for ```RAW``` tag
    }
}
