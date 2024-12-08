using System.Text;

namespace Markdown;

public class MarkdownToHtmlConverter : IMarkdownConverter
{
    private readonly Dictionary<TokenType, HtmlTokenConverter> typeToConverters;

    public MarkdownToHtmlConverter(IEnumerable<HtmlTokenConverter> converters)
    {
        typeToConverters = converters.ToDictionary(c => c.TypeOfToken);
    }

    public string Convert(List<Token> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        var result = new StringBuilder();

        foreach (var token in tokens)
            result.Append(ConvertToken(token));

        return result.ToString();
    }

    private string ConvertToken(Token token)
    {
        var converter = typeToConverters[token.Type];

        if (token is not ComplexToken)
            return converter.Convert(token);

        var result = new StringBuilder();

        result.Append(converter.OpenTag);
        foreach (var child in token.Childs)
        {
            result.Append(ConvertToken(child));
        }
        result.Append(converter.CloseTag);

        return result.ToString();
    }
}