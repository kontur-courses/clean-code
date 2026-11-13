using Markdown.EmphasisResolving;
using Markdown.Writers;

namespace Markdown;

public class Md(Tokenizer tokenizer, IWriter writer)
{
    public string Render(string markdown)
    {
        if (markdown == null)
            throw new ArgumentNullException(nameof(markdown));

        var tokens = tokenizer.Tokenize(markdown);
        EmphasisResolver.Resolve(markdown, tokens);
        return writer.Write(tokens);
    }
}