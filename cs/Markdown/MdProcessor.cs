using Markdown.Enums;
using Markdown.Tokens;
using System.Text;

namespace Markdown;

public static class MdProcessor
{
    public static string Render(string text)
    {
        var tokens = MarkdownTokenizer.TransformTextToTokens(text);
        return CombineString(tokens);
    }

    private static string CombineString(IList<MarkdownToken> tokens)
    {
        var builder = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.AssociatedTag == null)
            {
                builder.Append(token.TextContent);
                continue;
            }

            var tokenTag = token.AssociatedTag!;

            switch (tokenTag.Status)
            {
                case TagStatus.Broken:
                    builder.Append(tokenTag.Info.GlobalTag);
                    break;
                case TagStatus.Open:
                    builder.Append(tokenTag.Info.OpenTag);
                    break;
                default:
                    builder.Append(tokenTag.Info.CloseTag);
                    break;
            }
        }

        return builder.ToString();
    }
}