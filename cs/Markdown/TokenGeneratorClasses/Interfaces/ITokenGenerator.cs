using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses.Interfaces;

public interface ITokenGenerator
{
    public static abstract Token? GetToken(string line, int currentIndex);
}