using System.Text;

namespace Markdown;

public class Converter
{
    public string ConvertWithTokens(List<Token> tokens)
    {
        StringBuilder result = new();

        foreach (var token in tokens)
        {
            result.Append(token.Context);
        }

        return result.ToString();
    }
}