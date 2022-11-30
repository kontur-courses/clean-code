using System.Text;
using Markdown.Reading;

namespace Markdown.Parsing;

public class Match
{
    private readonly PatternTree _pattern;

    public int StartPosition { get; private set; }

    public int Lenght => _text.Length;
    public bool IsSuccess { get; private set; }

    private StringBuilder _text;
    public IMdTag SourceMdTag { get; private set; }
    public string Text => _text.ToString();


    public Match(IMdTag sourceMdTag, int startPosition, PatternTree pattern)
    {
        _text = new StringBuilder();
        _pattern = pattern;
        SourceMdTag = sourceMdTag;
        StartPosition = startPosition;
    }


    public MatchState CheckToken(Token token)
    {
        var previousState = _pattern.GetCurrentStateNodeTypes();
        var checkResult = _pattern.CheckToken(token);
        if (checkResult == MatchState.NotSuccess)
            return MatchState.NotSuccess;

        if (token.Symbol == '\0')
        {
        }

        var currentState = _pattern.GetCurrentStateNodeTypes();
        if (previousState.Any(x => x == StateNodeType.StartPosition))
        {
            // Lenght += 1;
            _text.Append(token.Symbol);
            StartPosition = token.Position;
        }

        if (previousState.Any(x => x == StateNodeType.Main) || previousState.Any(x => x == StateNodeType.Lookahead))
        {
            // Lenght += 1;
            _text.Append(token.Symbol);
        }


        if (currentState.Any(x => x == StateNodeType.End))
        {
            if (previousState.Any(x => x == StateNodeType.Lookahead))
                _text.Remove(_text.Length - 1, 1);
            IsSuccess = true;
        }

        if (checkResult == MatchState.FullMatch)
            IsSuccess = true;

        return checkResult;
    }
}