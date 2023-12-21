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

    public void RenderParameters(string parametersValues, IList<string> parameters)
    {
        var values = parametersValues.Split(" ", parameters.Count);
        var result = new StringBuilder();
        result.Append(OpeningSeparator.Substring(0, OpeningSeparator.Length - 1));
        for (var i = 0; i < parameters.Count; i++)
        {
            if (i >= values.Length)
                result.Append($" {parameters[i]}=\"\"");
            else result.Append($" {parameters[i]}=\"{values[i]}\"");
        }

        result.Append(">");
        OpeningSeparator = result.ToString();
    }
}