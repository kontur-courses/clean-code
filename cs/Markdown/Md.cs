using System.Text;

namespace Markdown;

public class Md
{
    public string Render(string markdownString)
    {
        var result = new StringBuilder();
        
        var paragraphs = ParagraphCreator.CreateParagraphs(markdownString);

        foreach (var paragraph in paragraphs)
        {
            var tokens = Tokenizer.CreateTokens(paragraph);
            var html = HtmlCreator.CreateHtml(tokens);
            result.Append(html);
        }
        
        return result.ToString();
    }
}