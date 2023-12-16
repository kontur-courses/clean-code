using System.Text;

namespace Markdown;

public static class Builder
{
    public static string Build(List<Token> tokens)
    {
        var text = new StringBuilder();
        foreach (var token in tokens) text.Append(token.Content);

        return text.ToString();
    }
}