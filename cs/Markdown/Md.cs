using Markdown.EmphasisResolving;
using Markdown.Writers;

namespace Markdown;

public class Md(Tokenizer tokenizer, IWriter writer, EmphasisResolver emphasisResolver)
{
    public string Render(string markdown)
    {
        if (markdown == null)
            throw new ArgumentNullException(nameof(markdown));

        var tokens = tokenizer.Tokenize(markdown);
        emphasisResolver.Resolve(tokens, markdown);
        return writer.Write(tokens);
    }
}