using System.Collections.Immutable;
using Markdown.Tags;
using Markdown.Tags.MarkdownTags;
using Markdown.Token;

namespace Markdown.Parse;

public static class MdRules
{
    private static readonly List<Tag> TagsThatCanIntersect;
    private static readonly List<Tag> SupportedTags;
    private static readonly List<char> SpecialChars;

    static MdRules()
    {
        SupportedTags = MarkdownTagProvider.SupportedTags;
        TagsThatCanIntersect = SupportedTags.Where(tag => tag.CanIntersect).ToList();
        SpecialChars = new List<char>();
        foreach (var tag in SupportedTags)
        {
            if (tag.Opening.Length > 0 && !SpecialChars.Contains(tag.Opening[0]))
                SpecialChars.Add(tag.Opening[0]);
            if (tag.Closing.Length > 0 && !SpecialChars.Contains(tag.Closing[0]))
                SpecialChars.Add(tag.Closing[0]);
        }
    }
    
    public static bool IsSpecialChar(char c)
    {
        return SpecialChars.Contains(c);
    }

    public static IEnumerable<Tag> GetPossibleIntersections(Tag tag)
    {
        return tag.CanIntersect 
            ? TagsThatCanIntersect.Where(intersection => intersection != tag) 
            : ImmutableArray<Tag>.Empty;
    }

    public static bool Intersects(Tag tag, Stack<TokenTree> stack, TokenTree root)
    {
        if (!tag.CanIntersect) return false;
        
        var father = stack.Count > 0 ? stack.Peek() : root;
        return GetPossibleIntersections(tag)
            .Any(intersection => father.Children
                .Where(child => child.IsTaggedWith(intersection) && child.Closed)
                .Any(child => child.HasChildrenWithOpenTag(tag)));
    }

    public static Tag ParseTag(Stack<TokenTree> stack, TokenTree currentNode, string text, int position, out int newPosition)
    {
        newPosition = position;
        switch (text[position])
        {
            case '_':
                if (position + 1 >= text.Length || text[position + 1] != '_')
                    return MarkdownTagProvider.Italics;
        
                if (position + 2 < text.Length && text[position + 1] == '_' && text[position + 2] == '_')
                {
                    if (currentNode.IsTaggedWith(MarkdownTagProvider.Bold))
                    {
                        newPosition++;
                        return MarkdownTagProvider.Bold;
                    }
        
                    if (currentNode.IsTaggedWith(MarkdownTagProvider.Italics))
                    {
                        return MarkdownTagProvider.Italics;
                    }
        
                    foreach (var tree in stack)
                    {
                        if (tree.IsTaggedWith(MarkdownTagProvider.Bold))
                        {
                            newPosition++;
                            return MarkdownTagProvider.Bold;
                        }
        
                        if (tree.IsTaggedWith(MarkdownTagProvider.Italics))
                        {
                            return MarkdownTagProvider.Italics;
                        }
                    }
                }
        
                newPosition++;
                return MarkdownTagProvider.Bold;
            case '#' when position + 1 < text.Length && text[position + 1] == ' ':
                newPosition++;
                return MarkdownTagProvider.Heading;
            default:
                throw new NotSupportedException();
        }
    }

    public static bool IsValidOpening(Tag tag, Stack<TokenTree> stack, string text, int position)
    {
        return tag.IsValidOpening(stack, text, position);
    }
    
    public static bool IsValidClosing(Tag tag, Stack<TokenTree> stack, string text, int position)
    {
        return tag.IsValidClosing(stack, text, position);
    }
}