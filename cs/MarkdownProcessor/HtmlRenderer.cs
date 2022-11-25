using System.Text;
using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public class HtmlRenderer : IRenderer
{
    private readonly Dictionary<TextType, string> tagNames;

    public HtmlRenderer(Dictionary<TextType, string> textTypeToTagName)
    {
        tagNames = textTypeToTagName;
    }

    public string Render(IEnumerable<ITag> tags, StringBuilder text)
    {
        var sb = new StringBuilder(text.ToString());

        var replacements = new List<(int, string, string)>();
        foreach (var tag in tags)
        {
            replacements.Add((
                tag.OpeningToken.TagFirstCharIndex,
                tag.Config.OpeningSign,
                $"<{tagNames[tag.Config.TextType]}>"));
            replacements.Add((
                tag.ClosingToken.TagFirstCharIndex, 
                tag.Config.ClosingSign, 
                $"</{tagNames[tag.Config.TextType]}>"));
        }

        var indexOffset = 0;
        foreach (var r in replacements.OrderBy(r => r.Item1))
        {
            sb.Replace(r.Item2, r.Item3, r.Item1 + indexOffset, r.Item2.Length);
            indexOffset += r.Item3.Length - r.Item2.Length;
        }

        return sb.ToString().Trim();
    }
}