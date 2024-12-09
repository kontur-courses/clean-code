using Markdown.Tags;
using Markdown.TokenGeneratorClasses.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses.TokenGeneratorRules;

public class GenerateHashTokenRule : ITokenGenerateRule
{
    public Token? GetToken(string line, int currentIndex)
    {
        if (currentIndex + 1 < line.Length && line[currentIndex] == '#' && line[currentIndex + 1] == ' ')
            return new Token(TokenType.MdTag, "# ", currentIndex, false, TagType.Header);

        return null;
    }
}