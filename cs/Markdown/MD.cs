using System.Text;

namespace Markdown;

public class Md
{
    private Parser parser = new();

    public string Render(string MarkdownText)
    {
        return RenderSubString(MarkdownText);
    }

    private string RenderSubString(string SubString)
    {
        var parsedString = parser.Parse(SubString);
        var renderedString = new StringBuilder();
        var renderedSubStrings = new List<RenderedString>();
        for (int i = 0; i < SubString.Length; i++)
        {
            var a = parsedString.Where(ps => ps.Start == i).ToList();
            if (a.Count != 0)
            {
                var ps = a[0];
                string text = SubString.Substring(ps.Start, ps.End - ps.Start);
                if (ps.Prefix.Equals("_") || ps.Prefix.Equals(""))
                    text = ps.Type.ConvertToHtml(text);
                else
                    text = RenderSubString(ps.Type.ConvertToHtml(text));
                renderedString.Append(text);
                i = ps.End;
            }
        }

        return renderedString.ToString();
    }
}