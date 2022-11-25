using System.Text;
using MarkdownProcessor.Renderer;
using MarkdownProcessor.Tags;

namespace MarkdownProcessor;

public class Md
{
    private readonly Dictionary<string, ITagMarkdownConfig> openedTokens;

    private readonly string[] tokensValues;

    public Md(IEnumerable<ITagMarkdownConfig> configs)
    {
        var configsArray = configs.ToArray();
        openedTokens = configsArray.ToDictionary(c => c.OpeningSign);
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
        var paragraphs = text.Split("\n");

        var closedTags = new List<ITag>();
        foreach (var paragraph in paragraphs)
        {
            var tags = new List<ITag>();
            foreach (var token in ExtractTokens(paragraph + '\n', result, tokensValues))
                if (tags.Count == 0 || tags.Last().Closed)
                {
                    var nullableTag = openedTokens.GetValueOrDefault(token.Value)?.CreateOrNull(token);
                    if (nullableTag is not null) tags.Add(nullableTag);
                }
                else
                {
                    var nullableToken = tags.Last().RunTokenDownOfTree(token);
                    if (nullableToken is null) continue;

                    var nullableTag = openedTokens.GetValueOrDefault(token.Value)?.CreateOrNull(token);
                    if (nullableTag is not null) tags.Last().RunTagDownOfTree(nullableTag);
                }


            closedTags.AddRange(tags.Concat(tags.SelectMany(GetAllChildren)).Where(t => t.Closed));
        }

        return renderer.Render(closedTags, result);
    }

    private static IEnumerable<ITag> GetAllChildren(ITag tag)
    {
        return tag.Children.Concat(tag.Children.SelectMany(GetAllChildren));
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