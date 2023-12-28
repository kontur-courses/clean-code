using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class TokenHighlighter
{
    public static List<Token> Excluded { get; } = new();

    private readonly List<Tag> tags = new()
    {
        new LinkTag(),
        new StrongTag(),
        new EmTag(),
        new HeaderTag()
    };

    public IEnumerable<Token> HighlightTokens(string markdownText)
    {
        var tokens = new List<Token>();

        var count = 0;
        for (var i = 0; i < markdownText.Length; i++)
        {
            if (tags.Any(tag => tokens.TryAddToken(tag, markdownText, i)))
            {
                if (count != 0)
                    tokens.Insert(tokens.Count - 1,
                        new StringToken(markdownText.Substring(i - count, count)));

                count = 0;

                var added = tokens.Last();
                i += added.Str.Length - 1;
                added.AddInner(HighlightTokens(added.GetBody()));
            }
            else count++;
        }

        if (count != 0)
            tokens.Add(new StringToken(markdownText.Substring(markdownText.Length - count, count)));

        return tokens;
    }
}