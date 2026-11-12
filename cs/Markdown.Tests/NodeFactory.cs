using Markdown.Domain;

namespace Markdown.Tests;

internal static class NodeFactory
{
    public static TextNode Text(string text) => new(text);

    public static ParagraphNode Paragraph(params Node[] children) => new() { Children = children };

    public static HeaderNode Header(params Node[] children) => new() { Children = children };

    public static EmphasisNode Emphasis(params Node[] children) => new() { Children = children };

    public static StrongNode Strong(params Node[] children) => new() { Children = children };

    public static UnorderedListNode UnorderedList(params Node[] children) => new() { Children = children };

    public static ListItemNode ListItem(params Node[] children) => new() { Children = children };
}