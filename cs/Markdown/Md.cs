using System.Text;
using Markdown.Token;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var parser = new TagParser();
        return GenerateHtml(text, parser.GetTokens(text));
    }

    private static string GenerateHtml(string text, List<IToken> tokens)
    {
        var currentTokens = tokens.OrderBy(t => t.Position).ToList();
        int position = 0;
        var sb = new StringBuilder();
        foreach (var token in currentTokens)
        {
            sb.Append(text[position..token.Position]);
            sb.Append(token.ConvertedText);
            position = token.Position + token.SourceText.Length > text.Length ? text.Length : token.Position + token.SourceText.Length;
        }

        sb.Append(text[position..text.Length]);

        return sb.ToString();
    }
}