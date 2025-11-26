using Markdown.Core.Lexing;

namespace Markdown.Core.Parsing;

public class ParserState
{
    private readonly IReadOnlyList<Token> _tokens;
    private IEnumerator<Token> _tokenPointer;

    public ParserState(IReadOnlyList<Token> tokens)
    {
        _tokens = tokens;
        _tokenPointer = _tokens.GetEnumerator();
    }

    public Token CurrentToken { get; private set; }
    public int CurrentIndex { get; private set; }

    public void Start()
    {
        CurrentIndex = 0;
        _tokenPointer = _tokens.GetEnumerator();
        MoveNext();
    }

    public Token MoveNext()
    {
        if (!_tokenPointer.MoveNext())
        {
            CurrentToken = new Token(TokenKind.Eof, ReadOnlyMemory<char>.Empty);
        }
        else
        {
            CurrentToken = _tokenPointer.Current;
            CurrentIndex++;
        }
        return CurrentToken;
    }

    public void SkipEmptyLines()
    {
        while (CurrentToken.Kind == TokenKind.NewLine)
            MoveNext();
    }

    public void SkipSpace()
    {
        if (CurrentToken.Kind == TokenKind.Space)
            MoveNext();
    }

    public bool IsEndOfLine() => CurrentToken.Kind is TokenKind.NewLine or TokenKind.Eof;

    public bool IsAtLineStart() => CurrentIndex <= 1 || _tokens[CurrentIndex - 2].Kind == TokenKind.NewLine;
}
