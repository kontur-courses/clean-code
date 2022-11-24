using System.Text;

namespace Markdown.Tags.Implementation;

public class NumericListMarkdownToHtml : ITag
{
    public string SourceName => "*";
    public string TranslateName => "!!!Numeric!!!";
    public string ResultName => "numeric";
    
    public string MakeTransformations(string substring)
    {
        var counter = 1;
        var sb = new StringBuilder();
        sb.AppendLine();
        foreach (var item in substring.Split(' '))
        {
            sb.AppendLine($"{counter++}. {item}");
        }

        return sb.ToString();
    }
}