using System.Text;

namespace Markdown.Primitives;

public class TagNode
{
    public readonly Tag Tag;
    public readonly TagNode[] Children;

    public TagNode(Tag tag, TagNode[] children)
    {
        Tag = tag;
        Children = children;
    }

    public TagNode(Tag tag, TagNode child) : this(tag, new[] { child })
    {
    }

    public TagNode(Tag tag) : this(tag, Array.Empty<TagNode>())
    {
    }

    public string ToText()
    {
        if (Children.Length == 0)
        {
            return Tag.Value;
        }

        var innerText = string.Join("", Children.Select(x => x.ToText()));
        return $"{Tag.Value}{innerText}{Tag.Value}";
    }

    public override bool Equals(object? obj)
    {
        return obj is TagNode node
               && Tag == node.Tag
               && Children.SequenceEqual(node.Children);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Tag.GetHashCode());
        foreach (var child in Children)
        {
            hash.Add(child.GetHashCode());
        }

        return hash.ToHashCode();
    }
}