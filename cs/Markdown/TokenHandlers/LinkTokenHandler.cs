using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown.TokenHandlers;

public class LinkTokenHandler : ITokenHandler
{
    public bool CanHandle(char current, char next, MarkdownParseContext context)
        => current == '[';

    public void Handle(MarkdownParseContext context)
    {
        MarkdownParser.AddToken(context, TokenType.Text);

        var startIndex = context.CurrentIndex;
        var endIndex = FindClosingBracket(context.MarkdownText, startIndex);
        if (endIndex == -1)
        {
            context.Buffer.Append(context.MarkdownText[startIndex]);
            context.CurrentIndex++;
            return;
        }

        var linkStartIndex = context.MarkdownText.IndexOf('(', endIndex);
        var linkEndIndex = context.MarkdownText.IndexOf(')', linkStartIndex);
        if (linkStartIndex == -1 || linkEndIndex == -1)
        {
            context.Buffer.Append(context.MarkdownText[startIndex]);
            context.CurrentIndex++;
            return;
        }

        var labelText = context.MarkdownText.Substring(startIndex + 1, endIndex - startIndex - 1);
        var url = context.MarkdownText.Substring(linkStartIndex + 1, linkEndIndex - linkStartIndex - 1);

        var labelTokens = context.Parser.ParseTokens(labelText);

        var linkToken = new LinkToken(labelTokens, url);
        if (context.Stack.Count > 0)
        {
            context.Stack.Peek().Children.Add(linkToken);
        }
        else
        {
            context.Tokens.Add(linkToken);
        }

        context.CurrentIndex = linkEndIndex + 1;
    }

    private static int FindClosingBracket(string text, int startIndex)
    {
        var depth = 0;
        for (var i = startIndex; i < text.Length; i++)
        {
            if (text[i] == '\\')
            {
                i++;
                continue;
            }
            if (text[i] == '[')
                depth++;
            else if (text[i] == ']')
                depth--;

            if (depth == 0)
                return i;
        }
        return -1;
    }
}