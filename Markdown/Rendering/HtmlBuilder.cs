using Markdown.Tokenization.Tokens;
using System.Text;

namespace Markdown.Rendering;

public class HtmlBuilder
{
    private readonly StringBuilder _sb = new();

    public void Append(ResolvedTokens tokens)
    {
        throw new NotImplementedException();
    }

    public string GetPage(string name)
        => $"""
            <!DOCTYPE html>
            <html lang="ru">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>{name}</title>
                </head>
                <body>
                    {ToString()}
                </body>
            </html>
            """;

    public override string ToString()
        => _sb.ToString();
}
