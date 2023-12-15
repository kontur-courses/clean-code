using Markdown.Helpers;
using System.Text;

namespace Markdown.Tokens;

public static class Tokenizer
{
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

            var tagToken = TryGetTagTokenOnPosition(i, text);

            if (tagToken != null)
            {
                if (collector.Length > 0)
                {
                    tokens.Add(new Token(GetPositionOffset(tokens), text: collector.ToString()));
                    collector.Clear();
                }

                tokens.Add(tagToken);
                i += tagToken.Tag!.Info.GlobalMark.Length - 1;

                continue;
            }

            collector.Append(text[i]);
        }

        if (collector.Length > 0)
            tokens.Add(new Token(GetPositionOffset(tokens), text: collector.ToString()));

        return tokens;
    }

    private static Token? TryGetTagTokenOnPosition(int position, string text)
    {
        Token? foundToken = null;
        
        var prefix = string.Concat(text[position], text[position + 1]);
        var foundTag = TagHelper.GetInstanceViaMark(prefix, position);

        if (foundTag != null)
            foundToken = new Token(position, foundTag);
        
        return foundToken;
    }

    // Usable for text-type tokens.
    private static int GetPositionOffset(List<Token> tokens)
    {
        if (tokens.Count == 0)
            return 0;

        var lastToken = tokens[^1];
        var lastTokenMark = lastToken.Tag!.Info.GlobalMark;

        return lastToken.Position + lastTokenMark.Length;
    }

    private static bool IsEscaped(char previous, char current)
    {
        return previous == '\\' && TagHelper.AvailableMarks.Contains(current);
    }
}