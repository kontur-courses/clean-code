using System.Text;
using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public class HtmlRenderer
{
    private readonly Dictionary<TextType, string> tagNames = new()
    {
        { TextType.Italic, "em" },
        { TextType.Bold, "strong" }
    };

    public string Render(IEnumerable<ITag> tags, StringBuilder stringBuilder)
    {
        var sb = new StringBuilder(stringBuilder.ToString());

        var replacements = new List<(int, string, string)>();
        foreach (var tag in tags)
        {
            replacements.Add((tag.StartIndex, tag.Config.Sign, $"<{tagNames[tag.Config.TextType]}>"));
            replacements.Add((tag.EndIndex, tag.Config.Sign, $"</{tagNames[tag.Config.TextType]}>"));
        }

        var indexOffset = 0;
        foreach (var r in replacements.OrderBy(r => r.Item1))
        {
            sb.Replace(r.Item2, r.Item3, r.Item1 + indexOffset, r.Item2.Length);
            indexOffset += r.Item3.Length - r.Item2.Length;
        }

        return sb.ToString();
    }
}