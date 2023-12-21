using Markdown.Interfaces;

namespace Markdown.Tags;

public class TagsRepository
{
    public static Dictionary<string, Func<IMarkdownTag>> GetCustomTags()
    {
        return new Dictionary<string, Func<IMarkdownTag>>
        {
            { "# ", () => new Header() },
            { "__", () => new Bold() },
            { "_", () => new Italic() },
            { "\\n", () => new Newline() },
            { "+", () => new ListItem() }
        };
    }

    public static bool IsTagBroken(IMarkdownTag markdownTag)
    {
        var isSurroundedByWhiteSpace = char.IsWhiteSpace(markdownTag.GetPreviousChar()) &&
                                       char.IsWhiteSpace(markdownTag.GetNextChar());
        var isSurroundedByDigits =
            char.IsDigit(markdownTag.GetPreviousChar()) && char.IsDigit(markdownTag.GetNextChar());

        return isSurroundedByWhiteSpace || isSurroundedByDigits;
    }
}