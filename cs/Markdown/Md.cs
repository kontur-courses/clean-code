using System.Text;
using Markdown.TagHandlers;

namespace Markdown;

public class Md
{
    private readonly ITagHandler[] tagHandlers;


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

    public string Render(string s)
    {
        return Render(s, tagHandlers);
    }

    public static string Render(string s, ITagHandler[] tagHandlers)
    {
        var builder = new StringBuilder();
        var index = 0;
        while (index < s.Length)
        {
            var tagHandler = tagHandlers.FirstOrDefault(x => x.StartsWithTag(s, index));
            if (tagHandler != null && tagHandler.IsValid(s, index))
            {
                var end = tagHandler.FindEndTagProcessing(s, index);
                var rendered = tagHandler.Render(s[..end], index);
                builder.Append(rendered);
                index += end - index;
            }
            else
            {
                builder.Append(s[index]);
                index++;
            }
        }

        return builder.ToString();
    }
}