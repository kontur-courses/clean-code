using Markdown.Parsing.Nodes;
using Markdown.Tokenizing.Tokens;

namespace Markdown.Parsing.Rules;

public class EscapeRule : IParsingRule
{
    public bool CanHandle(Token token) => token.Type == TokenType.EscapeCharacter;

    public void Handle(ParsingContext context)
    {
        context.MoveForward();

        if (context.IsEndOfTokens)
        {
            context.Nodes.Add(new TextNode { Content = "\\" });
            return;
        }
        
        var nextToken = context.CurrentToken;
        string content;

        switch (nextToken.Type)
        {
            case TokenType.Underscore:
                content = "_";
                context.MoveForward();
                break;
            case TokenType.Hashtag:
                content = "#";
                context.MoveForward();
                break;
            case TokenType.EscapeCharacter:
                content = "\\";
                context.MoveForward();
                break;
            default:
                content = "\\";
                break;
        }
        
        context.Nodes.Add(new TextNode { Content = content });
    }
}