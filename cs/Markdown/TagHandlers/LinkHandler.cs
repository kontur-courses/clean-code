namespace Markdown.TagHandlers;

public class LinkHandler : ITagHandler
{
    public KeyValuePair<string,string> MdHtmlTagPair => new("[]()", "<a>");

    public bool CanTransform(string text)
    {
        if (text.Length < 4 || text[0] is not '[')
            return false;

        var closeBracketIndex = FindEnd(text, '[', ']');
        if (closeBracketIndex == text.Length || closeBracketIndex == -1)
            return false;

        var linkText = text[closeBracketIndex..];
        if (linkText[0] is not '(')
            return false;

        var closeParenthesisIndex = closeBracketIndex + FindEnd(text[closeBracketIndex..], '(', ')');
        return closeBracketIndex < closeParenthesisIndex;
    }

    public StringManipulator Transform(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        if (!CanTransform(text))
            return StringManipulator.Default(text);

        var textEnd = FindEnd(text, '[', ']');
        var linkText = text.Substring(1, textEnd - 2);
        var link = text.Substring(textEnd + 1, FindEnd(text[textEnd..], '(', ')') - 2);
        var content = Format(linkText, link);
        return new StringManipulator(content, text[..(link.Length + linkText.Length + 4)], textEnd,
            link.Length + linkText.Length + 4);
    }

    private static string Format(string text, string link) => $"<a href=\"{link}\">{text}</a>";

    private static int FindEnd(string text, char opening, char closing)
    {
        if (text[0] != opening)
            throw new ArgumentException(nameof(text));

        var countOpened = 1;
        for (var i = 1; i < text.Length; i++)
        {
            if (text[i] == opening)
                countOpened++;
            else if (text[i] == closing)
                countOpened--;

            if (countOpened == 0)
                return i + 1;
        }

        return -1;
    }
}