using System.Text;
using MarkdownProcessor.Renderer;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor;

public class Md
{
    private readonly Dictionary<string, ITagMarkdownConfig> openingTokens;

    private readonly string[] tokensValues;

    public Md(IEnumerable<ITagMarkdownConfig> configs)
    {
        var configsArray = configs.ToArray();
        openingTokens = configsArray.ToDictionary(c => c.OpeningSign);
        tokensValues = configsArray
            .SelectMany(c => new[] { c.OpeningSign, c.ClosingSign })
            .Concat(new[] { " ", "\n" })
            .Distinct()
            .OrderByDescending(s => s)
            .ToArray();
    }

    public string Render(string text, IRenderer renderer)
    {
        var result = new StringBuilder();

        var tree = new TagsTree(openingTokens);
        var tokens = ExtractTokens(text + '\n', result, tokensValues);
        tree.ProcessTokens(tokens);

        return renderer.Render(tree.ClosedTags, result);
    }

    private static IEnumerable<Token> ExtractTokens(string text, StringBuilder resultText, string[] tokens)
    {
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '\\' && i + 1 < text.Length && ShieldedSymbol(text[i + 1], tokens))
            {
                resultText.Append(text[++i]);
                continue;
            }

            var matchedToken = tokens.FirstOrDefault(t => Match(t, i, text));

            if (matchedToken is null)
            {
                resultText.Append(text[i]);
                continue;
            }

            char? before = i == 0 ? null : text[i - 1];
            char? after = i + matchedToken.Length >= text.Length ? null : text[i + matchedToken.Length];
            yield return new Token(resultText.Length, matchedToken, before, after);

            resultText.Append(matchedToken);
            i += matchedToken.Length - 1;
        }
    }

    private static bool ShieldedSymbol(char c, IEnumerable<string> tokens)
    {
        return c == '\\' || tokens.Any(t => t.StartsWith(c));
    }

    private static bool Match(string expression, int index, string text)
    {
        return index + expression.Length <= text.Length &&
               expression.StartsWith(text[index]) &&
               text.Substring(index, expression.Length) == expression;
    }
}