using System.Text;

namespace Markdown.Extensions;

public static class TokenListExtensions
{
    private static bool InTag;

    public static string ConcatenateToString(this List<Token?> tokens)
    {
        var text = new StringBuilder();

        foreach (var token in tokens)
            if (token.IsTagEligible())
            {
                token.HandleTags();
                text.Append(token.Tag.TagContent);
            }
            else
            {
                text.Append(token.Content);
            }

        return text.ToString();
    }
}