using Markdown.Parsing.Nodes;
using Markdown.Tokenizing.Tokens;

namespace Markdown.Parsing.Rules;

public class HashtagRule : IParsingRule
{
    public bool CanHandle(Token token) => token.Type == TokenType.Hashtag;

    public void Handle(ParsingContext context)
    {
        context.Nodes.Add(new TextNode { Content = "#" });
        context.MoveForward();
    }
}