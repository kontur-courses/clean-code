using System.Text;
using Markdown.TokenGeneratorClasses.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses.TokenGeneratorRules;

public class GenerateTextTokenRule : ITokenGenerateRule
{
    private readonly IEnumerable<Func<string, int, Token?>> otherTokensRules;

    public GenerateTextTokenRule(IEnumerable<Func<string, int, Token?>> otherTokensRules)
    {
        this.otherTokensRules = otherTokensRules;
    }

    public Token? GetToken(string line, int currentIndex)
    {
        var stringBuilder = new StringBuilder();
        var tokenType = char.IsNumber(line[currentIndex]) ? TokenType.Number : TokenType.Text;

        for (var i = currentIndex; i < line.Length; i++)
        {
            if (tokenType == TokenType.Text &&
                (char.IsNumber(line[currentIndex]) || !IsTextToken(line, currentIndex)))
                break;

            if (tokenType == TokenType.Number && !char.IsNumber(line[currentIndex]))
                break;

            stringBuilder.Append(line[currentIndex]);
            currentIndex++;
        }

        var text = stringBuilder.ToString();

        return new Token(tokenType, text, currentIndex - text.Length);
    }

    private bool IsTextToken(string line, int currentIndex)
    {
        foreach (var rule in otherTokensRules)
        {
            var token = rule.Invoke(line, currentIndex);
            if (token != null)
                return false;
        }

        return true;
    }
}