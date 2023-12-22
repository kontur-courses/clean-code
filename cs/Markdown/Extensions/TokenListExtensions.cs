using System.Text;

namespace Markdown.Extensions;

public static class TokenListExtensions
{
    private static List<TagType> blockTags = new();

    public static string ConcatenateToString(this List<Token> tokens)
    {
        var text = new StringBuilder();

        foreach (var token in tokens)
        {
            if (IsTagEligible(token))
            {
                HandleTags(token);
                text.Append(token.Tag!.TagContent);
                continue;
            }

            if (token.Type == TokenType.LineBreaker) blockTags = new List<TagType>();
            text.Append(token.Content);
        }

        return text.ToString();
    }

    private static bool IsTagEligible(Token token)
    {
        return token.Type == TokenType.Tag && !blockTags.Contains(token.Tag!.TagType);
    }

    private static void HandleTags(Token token)
    {
        if (token.Tag != null && token.Tag.IsOpeningTag())
            blockTags.AddRange(token.Tag.ExcludedTags);
        else if (token.Tag != null && token.Tag.IsClosingTag()) blockTags = new List<TagType>();
    }
}