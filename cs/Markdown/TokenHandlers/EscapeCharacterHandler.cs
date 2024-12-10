using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.TokenHandlers;

public class EscapeCharacterHandler : ITokenHandler
{
    public bool CanHandle(char current, char next, MarkdownParseContext context)
        => current == '\\';

    public void Handle(MarkdownParseContext context)
    {

        if (context.CurrentIndex + 1 < context.MarkdownText.Length)
        {
            var next = context.MarkdownText[context.CurrentIndex + 1];
            if (next is '_' or '#' or '\\' or ']' or '(')
            {
                if (next != '\\')
                    context.Buffer.Append(next);
                context.CurrentIndex += 2;
                return;
            }
        }
        context.Buffer.Append('\\');
        context.CurrentIndex++;
    }
}