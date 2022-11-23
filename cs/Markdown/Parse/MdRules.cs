using System.Collections.Immutable;
using Markdown.Tags;
using Markdown.Token;

namespace Markdown.Parse;

public static class MdRules
{
    private static readonly HashSet<Tag> TagsThatCanIntersect = new()
    {
        MarkdownTags.Bold,
        MarkdownTags.Italics,
    };

    private static readonly Dictionary<Tag, HashSet<Tag>> PossibleIntersections = new()
    {
        { MarkdownTags.Bold, new HashSet<Tag> { MarkdownTags.Italics } },
        { MarkdownTags.Italics, new HashSet<Tag> { MarkdownTags.Bold } }
    };

    public static IEnumerable<Tag> GetPossibleIntersections(Tag tag)
    {
        return CanIntersect(tag) ? PossibleIntersections[tag] : ImmutableArray<Tag>.Empty;
    }

    public static bool CanIntersect(Tag tag) => TagsThatCanIntersect.Contains(tag);

    public static bool Intersects(Tag tag, Stack<TokenTree> stack, TokenTree root)
    {
        if (!CanIntersect(tag)) return false;
        
        var father = stack.Count > 0 ? stack.Peek() : root;
        return GetPossibleIntersections(tag)
            .Any(intersection => father.Children
                .Where(child => child.IsTaggedWith(intersection) && child.Closed)
                .Any(child => child.HasChildrenWithOpenTag(tag)));
    }

    public static Tag ParseTag(Stack<TokenTree> stack, TokenTree currentNode, string text, int position, 
        out int newPosition)
    {
        newPosition = position;
        switch (text[position])
        {
            case '_':
                if (position + 1 >= text.Length || text[position + 1] != '_') 
                    return MarkdownTags.Italics;
                
                if (position + 2 < text.Length && text[position + 1] == '_' && text[position + 2] == '_')
                {
                    if (currentNode.IsTaggedWith(MarkdownTags.Bold))
                    {
                        newPosition++;
                        return MarkdownTags.Bold;
                    }

                    if (currentNode.IsTaggedWith(MarkdownTags.Italics))
                    {
                        return MarkdownTags.Italics;
                    }

                    foreach (var tree in stack)
                    {
                        if (tree.IsTaggedWith(MarkdownTags.Bold))
                        {
                            newPosition++;
                            return MarkdownTags.Bold;
                        }
                        
                        if (tree.IsTaggedWith(MarkdownTags.Italics))
                        {
                            return MarkdownTags.Italics;
                        }
                    }
                }

                newPosition++;
                return MarkdownTags.Bold;
            case '#':
                return MarkdownTags.Heading;
            default:
                throw new NotSupportedException();
        }
    }

    public static bool IsValidOpening(Tag tag, Stack<TokenTree> stack, string text, int position)
    {
        if (tag.Equals(MarkdownTags.Bold)) return IsValidBoldOpening(stack, text, position);
        if (tag.Equals(MarkdownTags.Italics)) return IsValidItalicsOpening(stack, text, position);
        if (tag.Equals(MarkdownTags.Heading)) return IsValidHeadingOpening(stack, text, position);

        return false;
    }
    
    public static bool IsValidClosing(Tag tag, Stack<TokenTree> stack, string text, int position)
    {
        if (tag.Equals(MarkdownTags.Bold)) return IsValidBoldClosing(stack, text, position);
        if (tag.Equals(MarkdownTags.Italics)) return IsValidItalicsClosing(stack, text, position);

        return false;
    }
    
    private static bool IsValidBoldOpening(Stack<TokenTree> stack, string text, int position)
    {
        if (position + 1 >= text.Length || position + 2 >= text.Length) return false;
        if (text[position] != '_' || text[position + 1] != '_') return false;

        var nextChar = text[position + 2];
        if (nextChar is ' ' or '\r' or '\n') return false;
        
        if (position > 0 && char.IsDigit(text[position - 1]) && char.IsDigit(nextChar)) return false;
        if (stack.Where(tree => tree.Tag != null).Any(tree => tree.Tag.Equals(MarkdownTags.Italics))) return false;
        
        return true;
    }

    private static bool IsValidBoldClosing(Stack<TokenTree> stack, string text, int position)
    {
        if (position + 1 >= text.Length) return false;
        if (text[position] != '_' || text[position + 1] != '_') return false;
        if (position - 1 < 0) return false;
        
        var previousChar = text[position - 1];
        if (previousChar is ' ' or '\r' or '\n') return false;

        if (position + 2 < text.Length && char.IsDigit(previousChar) && char.IsDigit(text[position + 2])) 
            return false;

        return true;
    }

    private static bool IsValidItalicsOpening(Stack<TokenTree> stack, string text, int position)
    {
        if (position + 1 >= text.Length) return false;
        if (text[position] != '_') return false;

        var nextChar = text[position + 1];
        if (nextChar is ' ' or '\r' or '\n') return false;

        if (position > 0 && char.IsDigit(text[position - 1]) && char.IsDigit(nextChar)) return false;

        return true;
    }

    private static bool IsValidItalicsClosing(Stack<TokenTree> stack, string text, int position)
    {
        if (text[position] != '_') return false;
        if (position - 1 < 0) return false;
        
        var previousChar = text[position - 1];
        if (previousChar is ' ' or '\r' or '\n') return false;
        
        if (position + 1 < text.Length && char.IsDigit(previousChar) && char.IsDigit(text[position + 1])) 
            return false;

        return true;
    }

    private static bool IsValidHeadingOpening(Stack<TokenTree> stack, string text, int position)
    {
        if (position == 0) return true;
        if (text[position - 1] != '\n' && text[position - 1] != '\r') return false;

        return true;
    }
}