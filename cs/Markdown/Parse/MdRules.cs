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
                .Where(child => child.Tag == intersection && child.Closed)
                .Any(child => child.HasChildrenWithOpenTag(tag)));
    }

    public static Tag ParseTag(Stack<TokenTree> stack, string text, int position, out int newPosition)
    {
        foreach (var tag in SupportedTags.OrderByDescending(tag => tag.Opening.Length))
        {
            if (tag.IsOpeningSequence(text, position))
            {
                newPosition = tag.Opening.Length > 1 ? position + tag.Opening.Length - 1 : position;
                return tag;
            }
        }

        var openTags = stack
            .Where(node => node.Tag != null)
            .Select(node => node.Tag!);
        foreach (var tag in openTags)
        {
            if (tag.IsClosingSequence(text, position))
            {
                newPosition = tag.Closing.Length > 1 ? position + tag.Closing.Length - 1 : position;
                return tag;
            }
        }
        
        throw new NotSupportedException();
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