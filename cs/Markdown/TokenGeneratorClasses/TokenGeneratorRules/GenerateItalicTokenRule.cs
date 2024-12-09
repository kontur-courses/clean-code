using Markdown.Extensions;
using Markdown.Tags;
using Markdown.TokenGeneratorClasses.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenGeneratorClasses.TokenGeneratorRules;

public class GenerateItalicTokenRule : ITokenGenerateRule
{
    public Token? GetToken(string line, int currentIndex)
    {
        if (line[currentIndex] == '_' && !line.NextCharIs('_', currentIndex))
            return new Token(TokenType.MdTag, "_", currentIndex, false, TagType.Italic);

        return null;
    }
}