using System.Text;

namespace Markdown.Tag;

public class HtmlTag : ITag
{
    public string OpeningSeparator { get; private set; }
    public string CloseSeparator { get; }
    public bool IsPaired { get; }

    public HtmlTag(string openingSeparator, string endingSeparator, bool isPaired)
    {
        OpeningSeparator = openingSeparator;
        CloseSeparator = endingSeparator;
        IsPaired = isPaired;
    }

    public void RenderParameters(List<string> values, IList<string> parameters)
    {
        var result = new StringBuilder();
        result.Append(OpeningSeparator[..^1]);
        for (var i = 0; i < parameters.Count; i++)
        {
            if (i >= values.Count)
                result.Append($" {parameters[i]}=\"\"");
            else result.Append($" {parameters[i]}=\"{values[i]}\"");
        }

        result.Append(">");
        OpeningSeparator = result.ToString();
    }
}