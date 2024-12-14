using Markdown.Tags;

namespace Markdown;

public class Md
{
    private readonly List<ITagHandler> tagHandlers;

    public Md(List<ITagHandler>? customTagHandlers = null)
    {
        tagHandlers = customTagHandlers ?? new List<ITagHandler>
        {
            new BoldTag(),
            new ItalicTag(),
            new HeadingTag(),
            new EscapeTag(),
            new LinkTag(),
            new DefaultTagHandler()
        };
    }

    public string Render(string markdownString)
    {
        var index = 0;
        while (index < markdownString.Length)
            TryProcessTag(ref markdownString, ref index);

        return markdownString;
    }

    private void TryProcessTag(ref string markdownString, ref int index)
    {
        foreach (var handler in tagHandlers)
        {
            if (handler.IsTagStart(markdownString, index)){
                index = handler.ProcessTag(ref markdownString, index);
                break;
            }
        }
    }
}