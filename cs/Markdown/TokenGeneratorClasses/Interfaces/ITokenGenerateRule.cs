using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses.Interfaces;

public interface ITokenGenerateRule
{
    public Token? GetToken(string line, int currentIndex);
}