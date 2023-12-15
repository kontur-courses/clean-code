using System.Text;

namespace Markdown;

public static class Tokenizer
{
    private static readonly Dictionary<string, Tag> tagTemplates;

    static Tokenizer()
    {
        tagTemplates = new Dictionary<string, Tag>
        {
            { "__", new Tag("__", "<strong>", "</strong>", true) },
            { "_", new Tag("_", "<em>", "</em>", true) },
            { "# ", new Tag("# ", "<h1>", "</h1>", false) },
            { "\\n", new Tag("\\n", string.Empty, string.Empty, false) }
        };
    }

    public static List<Token> CollectTokens(string text)
    {
        var tokens = new List<Token>();
        var collector = new StringBuilder();

        text += " ";

        for (var i = 0; i < text.Length - 1; i++)
        {
            if (IsEscaped(text[i], text[i + 1]))
            {
                collector.Append(text[i + 1]);
                i += 1;

                continue;
            }

            var possibleToken = TryGetTokenOnPosition(text, i);

            if (possibleToken != null)
            {
                if (collector.Length > 0)
                {
                    tokens.Add(new Token(GetPositionOffset(tokens), text: collector.ToString()));
                    collector.Clear();
                }

                tokens.Add(possibleToken);
                i += possibleToken.Tag!.GlobalMark.Length - 1;

                continue;
            }

            collector.Append(text[i]);
        }

        if (collector.Length > 0)
            tokens.Add(new Token(GetPositionOffset(tokens), text: collector.ToString()));

        return tokens;
    }

    private static Token? TryGetTokenOnPosition(string text, int position)
    {
        Token? foundToken = null;

        foreach (var mark in tagTemplates.Keys)
        {
            var prefix = string.Concat(text[position], text[position + 1]);

            if (!prefix.StartsWith(mark))
                continue;

            var foundTag = tagTemplates[mark].Clone() as Tag;
            foundToken = new Token(position, foundTag);

            break;
        }

        return foundToken;
    }

    // Usable for text-type tokens.
    private static int GetPositionOffset(List<Token> tokens)
    {
        if (tokens.Count == 0)
            return 0;

        var lastToken = tokens[^1];
        var lastTokenMark = lastToken.Tag!.GlobalMark;

        return lastToken.Position + lastTokenMark.Length;
    }

    private static bool IsEscaped(char previous, char current)
    {
        return previous == '\\' && current is '_' or '#' or '\\';
    }
}