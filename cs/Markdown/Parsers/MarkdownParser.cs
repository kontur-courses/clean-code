using Markdown.Interfaces;
using Markdown.TokenHandlers;
using Markdown.Tokens;

namespace Markdown.Parsers;

public class MarkdownParser : IMarkdownParser
{
    private readonly List<ITokenHandler> handlers =
    [
        new StrongTokenHandler(),
        new HeaderTokenHandler(),
        new EmphasisTokenHandler(),
        new NewLineHandler(),
        new EscapeCharacterHandler(),
        new LinkTokenHandler()
    ];

    public IEnumerable<BaseToken> ParseTokens(string markdownText)
    {
        ArgumentNullException.ThrowIfNull(markdownText);

        var context = new MarkdownParseContext
        {
            MarkdownText = markdownText,
            Parser = this
        };

        while (context.CurrentIndex < context.MarkdownText.Length)
        {
            var current = context.MarkdownText[context.CurrentIndex];
            var next = context.CurrentIndex + 1 < context.MarkdownText.Length
                ? context.MarkdownText[context.CurrentIndex + 1]
                : '\0';

            var handler = handlers.FirstOrDefault(h => h.CanHandle(current, next, context));
            if (handler != null)
            {
                handler.Handle(context);
            }
            else
            {
                context.Buffer.Append(current);
                context.CurrentIndex++;
            }
        }

        AddToken(context, TokenType.Text);
        return context.Tokens;
    }

    public static void AddToken(MarkdownParseContext context, TokenType type)
    {
        if (context.Buffer.Length == 0) return;
        var token = new BaseToken(type, context.Buffer.ToString());
        context.Buffer.Clear();

        if (context.Stack.Count > 0)
            context.Stack.Peek().Children.Add(token);
        else
            context.Tokens.Add(token);
    }
}