using System.Collections.Generic;
using Markdown.Converter;
using Markdown.Tokenizer;
using Markdown.TokenParser;

namespace Markdown;
public class Md
{
    private readonly IMdTokenizer mdTokenizer;
    private readonly IMdTokenParser mdTokenParser;
    private readonly IMdConverter mdConverter;

    public Md(IMdTokenizer mdTokenizer, IMdTokenParser mdTokenParser, IMdConverter mdConverter)
    {
        this.mdTokenizer = mdTokenizer;
        this.mdTokenParser = mdTokenParser;
        this.mdConverter = mdConverter;
    }

    public string Render(string text)
    {
        var tokens = mdTokenizer.Tokenize(text);
        var parsed = mdTokenParser.Parse(tokens);
        var renderedText = mdConverter.RenderTokens(parsed);

        return renderedText;
    }
}
