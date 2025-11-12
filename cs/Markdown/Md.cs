using System.Text;
using Markdown.Data;

namespace Markdown;

public class Md
{
    public static string Render(string input)
    {
        var parser = new TokenParser();
        var tokens = parser.ParseTokens(input);

        return GenerateHtml(input, tokens);
    }

    public static string GenerateHtml(string text, IEnumerable<Token> tokens)
    {
        var mergedText = new StringBuilder();

        var tokenStack = new Stack<Token>();
        var prevPosition = 0;
        var outerEnd = text.Length + 3;
        foreach (var token in tokens)
        {
            if (token.StartPosition >= outerEnd)
            {
                while (tokenStack.Count > 0)
                {
                    var tokenPrev = tokenStack.Pop();
                    mergedText.Append(text.Substring(prevPosition,tokenPrev.EndPosition - prevPosition));
                    mergedText.Append(tokenPrev.Tail);
                    prevPosition = tokenPrev.EndPosition + tokenPrev.Gap(false);
                }
            }
            mergedText.Append(text.Substring(prevPosition,token.StartPosition-prevPosition));
            mergedText.Append(token.Head);
            prevPosition = token.StartPosition + token.Gap(true);
            outerEnd = token.EndPosition + token.Gap(false);
            tokenStack.Push(token);
        }

        while (tokenStack.Count > 0)
        {
            var token = tokenStack.Pop();
            mergedText.Append(text.Substring(prevPosition,token.EndPosition - prevPosition));
            mergedText.Append(token.Tail);
            prevPosition = token.EndPosition + token.Gap(false);
        }
        mergedText.Append(text.Substring(prevPosition, text.Length - prevPosition));
        return mergedText.ToString();
    }
}


