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

    public Match(Tag sourceTag, PatternTree pattern)
    {
        _text = new StringBuilder();
        _pattern = pattern;
        SourceTag = sourceTag;
    }


    public MatchState CheckToken(Token token)
    {
        var checkResult = _pattern.CheckToken(token);

        return MatchState.NotInitialized;
    }
}