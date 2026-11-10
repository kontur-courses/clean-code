using System.Collections.Generic;
using System.Text;

namespace Markdown;

public class HtmlParser(IParserFrom parserFrom) : IParserTo
{
    private readonly Dictionary<MarkupTokenType, string> markupTokenSymbols = new()
    {
        [MarkupTokenType.OpenBoldSymbol] = "<strong>",
        [MarkupTokenType.CloseBoldSymbol] = "</strong>",
        [MarkupTokenType.OpenItalicSymbol] = "<em>",
        [MarkupTokenType.CloseItalicSymbol] = "</em>",
        [MarkupTokenType.OpenHeaderSymbol] = "<h1>",
        [MarkupTokenType.CloseHeaderSymbol] = "</h1>"
    };

    public string Parse(string text)
    {
        var tokens = parserFrom.Parse(text);
        var result = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.Type == MarkupTokenType.Word)
                result.Append(token.Value);
            else
                result.Append(markupTokenSymbols[token.Type]);
        }

        return result.ToString();
    }
}