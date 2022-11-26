using Markdown.Interfaces;

namespace Markdown;

public class Md
{
    private readonly IConverter converter;
    private readonly ITokenizer tokenizer;

    public Md(IConverter converter, ITokenizer tokenizer)
    {
        this.converter = converter;
        this.tokenizer = tokenizer;
    }

    public string Render(string markdown)
    {
        if (markdown == null)
            throw new ArgumentNullException();

        var lines = markdown.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            tokenizer.Init(lines[i]);
            var tokens = tokenizer.TokenizeLine();
            lines[i] = converter.ConvertTokens(tokens);
        }

        return string.Join('\n', lines);
    }
}