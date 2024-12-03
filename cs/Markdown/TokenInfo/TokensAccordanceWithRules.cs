using Markdown.TagInfo;

namespace Markdown.TokenInfo;

internal static class TokensAccordanceWithRules
{
    private static readonly Dictionary<Tag, List<Tag>> DisallowedNesting = new()
    {
        { Tag.Italic, [Tag.Strong] }, { Tag.Link, [Tag.Strong, Tag.Italic, Tag.Link] }
    };

    public static IEnumerable<Token> Handle(IEnumerable<Token> tokens)
    {
        var selected = new List<Token>();
        var stack = new Stack<Token>();

        foreach (var token in tokens.OrderBy(x => x.Position))
        {
            if (token.Tag == Tag.Empty)
            {
                selected.Add(token);
            }
            else if (token.TagType == TagType.Open)
            {
                stack.Push(token);
            }
            else
            {
                var (opening, closing) = TryGetPair(stack, token);
                selected.Add(opening);
                selected.Add(closing);
            }
        }

        return selected.FindAll(x => x != null).OrderBy(x => x.Position);
    }

    private static bool IsAllowedNesting(Token parent, Token nested)
    {
        if (parent.Tag == Tag.Empty)
        {
            return true;
        }

        if (!DisallowedNesting.TryGetValue(parent.Tag, out var disallowed))
        {
            return true;
        }

        return !disallowed.Contains(nested.Tag);
    }

    private static (Token, Token) TryGetPair(Stack<Token> opened, Token closing)
    {
        if (opened.Count == 0)
        {
            return (null, null)!;
        }

        var opening = opened.Pop();

        if (opening.Tag != closing.Tag)
        {
            return (null, null)!;
        }

        if (opened.Count == 0 || IsAllowedNesting(opened.Peek(), opening))
        {
            return (opening, closing);
        }

        return (null, null)!;
    }
}