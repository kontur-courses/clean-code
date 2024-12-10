using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public class NewLineHandler : ITokenHandler
{
    public bool CanHandle(char current, char next, MarkdownParseContext context)
        => current == '\n' && context.Stack.Count > 0 && context.Stack.Peek().Type == TokenType.Header;

    public void Handle(MarkdownParseContext context)
    {
        MarkdownParser.AddToken(context, TokenType.Text);

        context.Tokens.Add(context.Stack.Pop());
        context.CurrentIndex++;
    }
}