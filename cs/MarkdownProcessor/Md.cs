using System.Text;
using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public class Md
{
    private readonly Dictionary<string, ITagMarkdownConfig> openedTokens = new()
    {
        { "_", new ItalicConfig() },
        { "__", new BoldConfig() },
        { "# ", new FirstHeaderConfig() }
    };

    private readonly HashSet<string> tokensValues = new() { "_", "__", "# ", " ", "\n" };

    public string Render(string text)
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
                    if (!openedTokens.ContainsKey(token.Value)) continue;

                    var nullableTag = openedTokens[token.Value].CreateOrNull(token);
                    if (nullableTag is not null) tags.Add(nullableTag);
                }
                else
                {
                    var nullableToken = tags.Last().RunTokenDownOfTree(token);
                    if (nullableToken is null) continue;

                    if (!openedTokens.ContainsKey(token.Value)) continue;

                    var nullableTag = openedTokens[token.Value].CreateOrNull(token);
                    if (nullableTag is not null) tags.Last().RunTagDownOfTree(nullableTag);
                }


            closedTags.AddRange(tags
                .Concat(tags.SelectMany(GetAllChildren))
                .Where(t => t.Closed)
                .ToArray());
        }

        return new HtmlRenderer().Render(closedTags, result);
    }

    private static IEnumerable<ITag> GetAllChildren(ITag tag)
    {
        return tag.Children.Concat(tag.Children.SelectMany(GetAllChildren));
    }

    private static IEnumerable<Token> ExtractTokens(string text, StringBuilder resultText, IEnumerable<string> tokens)
    {
        var tokensList = tokens.Distinct().OrderByDescending(s => s).ToArray();
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '\\')
                if (i + 1 < text.Length && (text[i + 1] == '\\' || tokensList.Any(t => t.StartsWith(text[i + 1]))))
                {
                    resultText.Append(text[++i]);
                    continue;
                }

            var token = tokensList
                .FirstOrDefault(t => i + t.Length <= text.Length &&
                                     t.StartsWith(text[i]) &&
                                     text.Substring(i, t.Length) == t);
            if (token != null)
            {
                char? before = i == 0 ? null : text[i - 1];
                char? after = i + token.Length >= text.Length ? null : text[i + token.Length];
                yield return new Token(resultText.Length, token, before, after);
                resultText.Append(token);
                i += token.Length - 1;
            }
            else
            {
                resultText.Append(text[i]);
            }
        }
    }
}