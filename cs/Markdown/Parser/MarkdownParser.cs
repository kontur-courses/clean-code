

using System.Text;
using Markdown.Parser.Interface;
using Markdown.Parser.TokenHandler;
using Markdown.Token;

namespace Markdown.Parser;

public class MarkdownParser : IMarkdownParser
{
    private readonly IList<ITokenHandler> handlers;

    public MarkdownParser()
    {
        handlers = TokenHandlerFactory.CreateHandlers();
    }

    public IEnumerable<Token.Token> Parse(string text)
    {
        var tokens = new List<Token.Token>();
        var openTags = new Stack<TokenType>();
        var textBuffer = new StringBuilder();
        var textStart = 0;
        var position = 0;

        if (text is null)
        {
            return tokens;
        }
        
        while (position < text.Length)
        {
            var context = new ParsingContext(text, position, openTags);
            var handled = false;

            foreach (var handler in handlers)
            {
                if (!handler.TryHandle(context, out var token, out var skip))
                {
                    continue;
                }
                
                if (textBuffer.Length > 0)
                {
                    tokens.Add(Token.Token.CreateText(textBuffer.ToString(), textStart));
                    textBuffer.Clear();
                }

                tokens.Add(token);
                position += skip;
                handled = true;
                break;
            }

            if (handled)
            {
                continue;
            }
            
            if (textBuffer.Length == 0)
                textStart = position;
            textBuffer.Append(text[position]);
            position++;
        }

        if (textBuffer.Length > 0)
            tokens.Add(Token.Token.CreateText(textBuffer.ToString(), textStart));

        return tokens;
    }
}