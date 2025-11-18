using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

internal class Lexer
{
    private readonly TokenFactory _tokenFactory;
    private readonly string _sourceText;

    public Lexer(string text, IEnumerable<ITag> tags)
    {
        _sourceText = text;
        _tokenFactory = new TokenFactory(text, tags);
    }

    public LinkedList<IToken> Tokenize()
    {
        var tokens = new LinkedList<IToken>();
        var lastTokenIndex = 0;
        
        for (var i = 0; i < _sourceText.Length;)
        {
            if (_tokenFactory.TryCreateToken(i, out var token))
            {
                if (i - lastTokenIndex > 0)
                    tokens.AddLast(new TextToken(_sourceText[lastTokenIndex..i]));

                tokens.AddLast(token);
                lastTokenIndex = i + token.Length;
                i = lastTokenIndex;
            }
            else
                i++;
        }
        
        if (_sourceText.Length - lastTokenIndex > 0)
            tokens.AddLast(new TextToken(_sourceText[lastTokenIndex..]));
        
        tokens.AddLast(new EOFToken());
        return tokens;
    }
}