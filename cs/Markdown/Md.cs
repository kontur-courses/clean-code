using System.Text;
using Markdown.Converters;
using Markdown.Tokenizers;

namespace Markdown;

public class Md(ITokenizer tokenizer, IConverter converter)
{
    public string Render(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;
            
        var result = new StringBuilder();
        var paragraphs = markdown.Split(Environment.NewLine);
            
        foreach (var paragraph in paragraphs)
        {
            var htmlLine = ProcessParagraph(paragraph);
            result.AppendLine(htmlLine);
        }

        return result
            .ToString()
            .TrimEnd(Environment.NewLine.ToCharArray());
    }

    private string ProcessParagraph(string line)
    {
        var trimmedLine = line.TrimEnd(Environment.NewLine.ToCharArray());

        if (trimmedLine.StartsWith("# "))
        {
            var headerContent = trimmedLine[2..];
            var htmlContent = ParseMarkdown(headerContent);
            return Tag.Wrap("h1", htmlContent);
        }
        else
        {
            var htmlContent = ParseMarkdown(trimmedLine);
            return htmlContent;
        }
    }

    private string ParseMarkdown(string markdown)
    {
        var tokens = tokenizer.Tokenize(markdown);
        var html = converter.Convert(tokens);
        return html;
    }
}