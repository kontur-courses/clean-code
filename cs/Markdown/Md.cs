using System.Text;

namespace Markdown;

public class Md
{

    
    private const string HeaderPrefix = "# ";
    
    /// <summary>
    /// Точка входа, использует ParagraphSplitter (деление на абзацы),
    /// Tokenizer, EmphasisResolver, HtmlWriter (разметка абзаца).
    /// </summary>
    public string Render(string markdown)
    {
        var result = new StringBuilder();
        foreach (var segment in ParagraphSplitter.Split(markdown))
        {
            result.Append(segment.Gap);
            result.Append(RenderParagraph(segment.Text));
        }
        return result.ToString();
    }
    
    private static string RenderParagraph(string paragraph)
    {
        if (paragraph.StartsWith(HeaderPrefix, StringComparison.Ordinal))
            return "<h1>" + RenderInline(paragraph.Substring(HeaderPrefix.Length)) + "</h1>";

        return RenderInline(paragraph);
    }
    
    private static string RenderInline(string text)
    {
        var tokens = Tokenizer.Tokenize(text);
        EmphasisResolver.Resolve(text, tokens);
        return HtmlWriter.Write(tokens);
    }
}