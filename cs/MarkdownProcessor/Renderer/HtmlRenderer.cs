using System.Text;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor.Renderer;

public class HtmlRenderer : IRenderer
{
    private readonly Dictionary<TextType, string> tagNames;

    public HtmlRenderer(Dictionary<TextType, string> textTypeToTagName)
    {
        tagNames = textTypeToTagName;
    }

    public string Render(IEnumerable<ITag> tags, StringBuilder text)
    {
        var replacements = new List<(int, string, string)>();
        foreach (var tag in tags)
        {
            if (!tagNames.ContainsKey(tag.Config.TextType)) continue;

            replacements.Add((
                tag.OpeningToken.TagFirstCharIndex,
                tag.OpeningToken.Value,
                $"<{tagNames[tag.Config.TextType]}>"));
            replacements.Add((
                tag.ClosingToken.TagFirstCharIndex,
                tag.ClosingToken.Value,
                $"</{tagNames[tag.Config.TextType]}>"));
        }

        var indexOffset = 0;
        foreach (var r in replacements.OrderBy(r => r.Item1))
        {
            text.Replace(r.Item2, r.Item3, r.Item1 + indexOffset, r.Item2.Length);
            indexOffset += r.Item3.Length - r.Item2.Length;
        }

        return text.ToString().Trim();
    }
}