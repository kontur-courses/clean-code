using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class TextRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var textLength = tokens
            .Skip(begin)
            .TakeWhile(IsText)
            .Count();
        return textLength == 0 ? null : new TextNode(begin, textLength);
    }

    private static bool IsText(Token token) 
        => token.TokenType is TokenType.Word or TokenType.Space;
}