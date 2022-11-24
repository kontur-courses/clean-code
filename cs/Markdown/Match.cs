using System.Text;

namespace Markdown;

public class Match
{
    private readonly PatternTree _pattern;
    public int StartPosition { get; private set; }
    public int Lenght { get; private set; }
    public bool IsSuccess { get; private set; }

    private StringBuilder _text;
    public Tag SourceTag { get; private set; }
    public string Text => _text.ToString();

    public Match(Tag sourceTag, int startPosition, PatternTree pattern)
    {
        _text = new StringBuilder();
        _pattern = pattern;
        SourceTag = sourceTag;
        StartPosition = startPosition;
    }


    public MatchState CheckToken(Token token)
    {
        var previousState = _pattern.StateNodeType;
        var checkResult = _pattern.CheckToken(token);
        if (checkResult == MatchState.NotSuccess)
            return MatchState.NotSuccess;

        var currentState = _pattern.StateNodeType;
        if (previousState == StateNodeType.StartPosition)
        {
            Lenght += 1;
            _text.Append(token.Symbol);
            StartPosition = token.Position;
        }

        if (previousState == StateNodeType.Main || (previousState != StateNodeType.Lookahead && currentState == StateNodeType.End))
        {
            Lenght += 1;
            _text.Append(token.Symbol);
        }

        if (_pattern.StateNodeType == StateNodeType.End)
        {
            IsSuccess = true;
        }

        if (checkResult == MatchState.FullMatch)
            IsSuccess = true;

        return checkResult;
    }
}