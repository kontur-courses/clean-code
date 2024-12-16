using Markdown.Parser.Nodes;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TextRules;

public class TextRule(List<TokenType> tokenTypes) : IParsingRule
{
    public TextRule() 
        : this([TokenType.WORD, TokenType.SPACE])
    { }
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var textLength = tokens.Skip(begin).TakeWhile(IsText).Count();
        return textLength == 0 ? null : new TextNode(begin, textLength);
    }

    private bool IsText(Token token) => tokenTypes.Contains(token.TokenType);
}