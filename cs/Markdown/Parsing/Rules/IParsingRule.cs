using Markdown.Tokenizing.Tokens;

namespace Markdown.Parsing.Rules;

public interface IParsingRule
{
    bool CanHandle(Token token);
    void Handle(ParsingContext context);
}