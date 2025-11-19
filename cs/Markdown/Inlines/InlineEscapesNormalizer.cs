using Markdown.Entities;
using System.Text;
using static Markdown.Inlines.InlineSyntax;

namespace Markdown.Inlines;

public static class InlineEscapesNormalizer
{
    public static string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var result = new StringBuilder(text.Length);

        for (var index = 0; index < text.Length;)
        {
            var current = text[index];

            if (current == Escape)
            {
                index = HandleEscape(text, index, result);
                continue;
            }

            result.Append(current);
            index++;
        }

        return result.ToString();
    }

    private static int HandleEscape(string text, int index, StringBuilder result)
    {
        if (index + 1 >= text.Length)
        {
            result.Append(Escape);
            return index + 1;
        }

        var nextSymbol = text[index + 1];

        if (nextSymbol == Underscore)
        {
            result.Append(PlaceholderUnderscore);
            return index + 2;
        }

        if (nextSymbol == Sharp)
        {
            result.Append(PlaceholderHash);
            return index + 2;
        }

        if (nextSymbol == Escape)
            return HandleEscapedEscape(text, index, result);

        result.Append(Escape).Append(nextSymbol);
        return index + 2;
    }

    private static int HandleEscapedEscape(string text, int index, StringBuilder result)
    {
        var afterSymbol = index + 2 < text.Length ? text[index + 2] : EndOfText;

        if (afterSymbol == Underscore || afterSymbol == Sharp)
            result.Append(PlaceholderBackslash);
        else
            result.Append(Escape).Append(Escape);

        return index + 2;
    }

    public static void RestorePlaceholders(List<Node> nodes)
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var text = nodes[i].Text;
            if (text is null)
                continue;

            var restored = text
                .Replace(PlaceholderUnderscore, Underscore)
                .Replace(PlaceholderBackslash, Escape)
                .Replace(PlaceholderHash, Sharp);

            if (!ReferenceEquals(restored, text))
                nodes[i] = new Node(restored, nodes[i].Type);
        }
    }
}
