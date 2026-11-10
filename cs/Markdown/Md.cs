using Markdown.Tokens;
using System.Text;

namespace Markdown;

public class Md(Tokenizer tokenizer)
{
    public string Render(string markdown)
    {
        if (markdown == null)
            throw new ArgumentNullException(nameof(markdown));

        var tokens = tokenizer.Tokenize(markdown);
        EmphasisResolver.Resolve(markdown, tokens);
        return WriteHtml(tokens);
    }

    private static string WriteHtml(IEnumerable<Token> tokens)
    {
        var result = new StringBuilder();
        foreach (var token in tokens)
        {
            result.Append(token.WriteHtml());
        }
        return result.ToString();
    }
}
