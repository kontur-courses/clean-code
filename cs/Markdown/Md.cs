using System.Text;
using Markdown.Extensions;
using Markdown.TagHandlers;

namespace Markdown;

public class Md : IMarkDown
{
    private readonly Dictionary<string, ITagHandler> handlers = new()
    {
        ["["] = new LinkHandler(),
        ["#"] = new TopLevelHeadingHandler(),
        ["_"] = new PairedTagHandler("_", "<em>"),
        ["__"] = new PairedTagHandler("__", "<strong>"),
    };

    public string Render(string text)
    {
        if (text is null)
            throw new ArgumentNullException(text);

        var builder = new StringBuilder(text.Length);
        var index = 0;
        while (index < text.Length)
        {
            if (text[index] is '\\')
            {
                builder.AppendEscapedCharacter(text, index);
                index += 2;

                continue;
            }

            var substring = text[index..];
            var handler = FindHandler(substring);
            if (handler is not null && handler.CanTransform(substring))
            {
                var str = handler.Transform(substring);
                var innerString = Render(str.GetInnerString());
                str.ReplaceInnerString(innerString);
                builder.Append(str.Content);
                index += str.OldString.Length;
            }
            else
            {
                builder.Append(text[index]);
                index += 1;
            }
        }

        return builder.ToString();
    }

    private ITagHandler? FindHandler(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        if (2 <= text.Length && handlers.TryGetValue(text[..2], out var handler))
            return handler;

        if (1 <= text.Length && handlers.TryGetValue(text[0].ToString(), out handler))
            return handler;

        return null;
    }
}