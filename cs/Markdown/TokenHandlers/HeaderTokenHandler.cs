using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public class HeaderTokenHandler : ITokenHandler
{
    public bool CanHandle(char current, char next, MarkdownParseContext context) 
        => current == '#' && (context.CurrentIndex == 0 || 
                              context.MarkdownText[context.CurrentIndex - 1] == '\n');

    public void Handle(MarkdownParseContext context)
    {
        while (context.CurrentIndex < context.MarkdownText.Length && 
               context.MarkdownText[context.CurrentIndex] == '#')
        {
            context.HeaderLevel++;
            context.CurrentIndex++;
        }

        if (context.CurrentIndex < context.MarkdownText.Length && 
            context.MarkdownText[context.CurrentIndex] == ' ')
        {
            context.CurrentIndex++;

            MarkdownParser.AddToken(context, TokenType.Text);
            var headerToken = new BaseToken(TokenType.Header)
            {
                HeaderLevel = context.HeaderLevel
            };

            context.Tokens.Add(headerToken);

            var headerEnd = context.MarkdownText.IndexOf('\n', context.CurrentIndex);
            if (headerEnd == -1)
                headerEnd = context.MarkdownText.Length;

            var headerContent = context.Parser
                .ParseTokens(context.MarkdownText[context.CurrentIndex..headerEnd]);

            foreach (var childToken in headerContent)
            {
                headerToken.Children.Add(childToken);
            }
            context.CurrentIndex = headerEnd;
        }
        else
        {
            context.Buffer.Append('#', context.HeaderLevel);
        }
    }
}