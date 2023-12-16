using System.Text;
using Markdown.TagHandlers;

namespace Markdown;

public class Md
{
    public ITagHandler[] tagHandlers;


    public Md()
    {
        tagHandlers = new ITagHandler[]
        {
            new HeadingHandler(),
            new BoldTagHandler(),
            new ItalicTagHandler(),
            new LinkTagHandler(),
        };
    }

    public string Render(string text)
    {
        return Render(text, tagHandlers);
    }

    public static string Render(string text, ITagHandler[] tagHandlers)
    {
        if (string.IsNullOrEmpty(text)) return "";

        var builder = new StringBuilder();
        var index = 0;
        while (index < text.Length)
        {
            if (IsEscaped(text, index))
            {
                AppendEscapedCharacter(text, index, builder);
                index += 2;
                continue;
            }

            if (TryHandleTag(text, index, tagHandlers, out var renderedContent, out var endIndex))
            {
                builder.Append(renderedContent);
                index = endIndex;
            }
            else
            {
                builder.Append(text[index]);
                index++;
            }
        }

        return builder.ToString();
    }

    private static bool IsEscaped(string text, int index)
    {
        return text[index] == '\\' && index < text.Length - 1;
    }

    private static void AppendEscapedCharacter(string text, int index, StringBuilder builder)
    {
        builder.Append(Escape(text.Substring(index, 2)));
    }

    private static bool TryHandleTag(string text, int index, ITagHandler[] tagHandlers, out string renderedContent, out int endIndex)
    {
        var tagHandler = FindHandler(text, index, tagHandlers);
        if (tagHandler != null && tagHandler.IsValid(text, index))
        {
            endIndex = tagHandler.FindEndTagProcessing(text, index);
            renderedContent = tagHandler.Render(text[..endIndex], index);
            return true;
        }

        renderedContent = null;
        endIndex = -1;
        return false;
    }

    public static ITagHandler? FindHandler(string text, int startIndex, ITagHandler[] tagHandlers)
    {
        return tagHandlers
            .Where(x => x.StartsWithTag(text, startIndex))
            .Where(x => x.IsValid(text, startIndex))
            .OrderByDescending(x => x.MdTag.Length)
            .FirstOrDefault();
    }

    public static string Escape(string s)
    {
        if (s == @"\\") return @"\";
        if (s.StartsWith(@"\")) return s[1..];
        return s;
    }
}

