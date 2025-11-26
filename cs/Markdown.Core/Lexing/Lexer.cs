using Markdown.Core.Lexing.Handlers;

namespace Markdown.Core.Lexing;

public class Lexer : ILexer
{
    private readonly List<ITokenHandler> _handlers =
    [
        new HashTokenHandler(),
        new EscapeTokenHandler(),
        new UnderscoreTokenHandler(),
        new SpaceTokenHandler(),
        new NewLineTokenHandler(),
        new BracketTokenHandler(),
        new ParenthesisTokenHandler()
    ];
    private readonly ITokenHandler _textHandler = new TextTokenHandler();

    public IEnumerable<Token> Tokenize(ReadOnlyMemory<char> source)
    {
        var tokens = new List<Token>();
        var i = 0;
        var length = source.Length;

        while (i < length)
        {
            var handled = false;
            foreach (var consumed in from handler in _handlers 
                     where handler
                         .CanHandle(source.Span, i) select handler
                         .Handle(source, i, tokens))
            {
                i += consumed;
                handled = true;
                break;
            }
            if (handled)
                continue;
            
            i += _textHandler.Handle(source, i, tokens);
        }
        tokens.Add(new Token(TokenKind.Eof, source[..0]));
        return tokens;
    }
}
