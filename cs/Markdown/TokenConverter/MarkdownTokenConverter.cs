using System.Text;
using Markdown.Lexer;

namespace Markdown.TokenConverter;

public class MarkdownTokenConverter : ITokenConverter
{
    public string ConvertToString(TokenizeResult tokenizeResult)
    {
        var stringBuilder = new StringBuilder();
        var appendToEnd = "";

        foreach (var token in tokenizeResult.Tokens)
        {
            if (token.Type.HasLineBeginningSemantics)
                appendToEnd = token.Type.Representation(true);

            if (token.Type.HasPredefinedValue)
            {
                stringBuilder.Append(token.GetRepresentation());
                continue;
            }

            for (var i = 0; i < token.Type.Value.Length; i++)
            {
                var isEscapeSymbol =
                    tokenizeResult.EscapeSymbolsPos.TryGetValue(token.StartingIndex + i, out var toIgnore);
                if (isEscapeSymbol && toIgnore)
                    continue;
                stringBuilder.Append(token.Type.Value[i]);
            }
        }

        stringBuilder.Append(appendToEnd);
        return stringBuilder.ToString();
    }
}