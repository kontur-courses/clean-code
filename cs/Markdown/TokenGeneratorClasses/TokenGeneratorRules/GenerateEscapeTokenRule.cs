using Markdown.TokenGeneratorClasses.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses.TokenGeneratorRules;

public class GenerateEscapeTokenRule : ITokenGenerateRule
{
    public Token? GetToken(string line, int currentIndex)
    {
        if (line[currentIndex] == '\\')
            return new Token(TokenType.Escape, @"\", currentIndex);

        return null;
    }
}