using Markdown.Tags;

namespace Markdown.Token;

public static class TokenTreeExtensions
{
    public static bool IsTaggedWith(this TokenTree tree, Tag tag)
    {
        return tree.Tag != null && tree.Tag.Equals(tag);
    }

    public static IEnumerable<TokenTree> TaggedChildren(this TokenTree tree)
    {
        return tree.Children.Where(child => child.Tag != null);
    }

    public static bool HasChildrenWithOpenTag(this TokenTree tree, Tag tag)
    {
        return tree.TaggedChildren()
            .Where(child => !child.Closed)
            .Any(child => child.IsTaggedWith(tag));
    }
}