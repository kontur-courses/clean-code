using Markdown.Tokens;
using System.Text;

namespace Markdown;

public class Md(Tokenizer tokenizer, HtmlWriter htmlWriter)
{
    public string Render(string markdown)
    {
        if (markdown == null)
            throw new ArgumentNullException(nameof(markdown));

        var tokens = tokenizer.Tokenize(markdown);
        EmphasisResolver.Resolve(markdown, tokens);
        return htmlWriter.Write(tokens);
    }
}