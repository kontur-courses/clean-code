using System.Text;
using Markdown.Tokens;

namespace Markdown;

public class Md
{
    public string Render(string text)
    {
        var result = new StringBuilder();

        var lines = text.Split('\n');
        var converter = new MarkdownToHtmlConverter();
        var tokens = lines.Select(MarkdownParser.ParseLine).ToList<Token>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.NestedTokens.Count <= 0 || token.NestedTokens[0] is not LineToken { IsStackable: true } t)
                continue;
            var multilineTokenType = LineToken.GetContainerTypeOrDefault(t);
            if (multilineTokenType is null)
                continue;
            var multilineToken = (MultilineToken?)Activator.CreateInstance(multilineTokenType);
            if (multilineToken is null)
                continue;

            var j = i;
            for (; j < tokens.Count; j++)
            {
                if (token.GetType() != tokens[j].GetType())
                    break;

                Nesting.AddToToken(tokens[j], multilineToken);
            }

            tokens.RemoveRange(i, j - i);
            tokens.Insert(i, multilineToken);
            i = j - 1;
        }

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            result.Append(converter.ToHtml(token));
            if (i < tokens.Count - 1)
                result.Append('\n');
        }

        return result.ToString();
    }
}