using System.Text;

namespace Markdown;

public class Md
{
    public string Render(string markdownString)
    {
        var processor = new MarkdownProcessor();
        
        return processor.ConvertToHtml(markdownString);
    }
}