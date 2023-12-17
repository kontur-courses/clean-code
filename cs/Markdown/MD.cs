using Markdown.Tags;
using Markdown.Tokens;
using System.Text;

namespace Markdown;

public static class MD
{
    public static string Render(string text)
    {
        var tokens = Tokenizer.CollectTokens(text);
        return CombineString(tokens);
    }

    private static string CombineString(IList<Token> tokens)
    {
        var builder = new StringBuilder();

        foreach (var token in tokens)
        {
            if (token.Tag == null)
            {
                builder.Append(token.Text);
                continue;
            }

            var tokenTag = token.Tag!;

            switch (tokenTag.Status)
            {
                case TagStatus.Broken:
                    builder.Append(tokenTag.Info.GlobalMark);
                    break;
                case TagStatus.Open:
                    builder.Append(tokenTag.Info.OpenMark);
                    break;
                default:
                    builder.Append(tokenTag.Info.CloseMark);
                    break;
            }
        }

        return builder.ToString();
    }
}