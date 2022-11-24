namespace Markdown;

public class MarkdownParser
{
    private readonly List<Tag> _tags;

    private List<Match> _processMatches;

    public List<Match> Matches;
    public IEnumerable<Tag> Tags => _tags;

    public MarkdownParser(List<Tag> tags)
    {
        _tags = tags;
        _processMatches = new List<Match>();
        Matches = new List<Match>();
    }

    public MarkdownParser()
    {
        _tags = new List<Tag>();
        _processMatches = new List<Match>();
        Matches = new List<Match>();
    }

    public void Parse(string text)
    {
        Matches.Clear();
        var reader = new TextReader(text);

        while (reader.ReadNextToken())
        {
            var currentToken = reader.Current;

            if (currentToken == null)
                break;

            foreach (var tagToCheck in _tags)
            {
                var mathPatterns = tagToCheck.CheckFirstCharMatch(currentToken!);
                if (mathPatterns.Any())
                {
                    mathPatterns.ForEach(pattern => { _processMatches.Add(new Match(tagToCheck, currentToken.Position, pattern.CopyPatternTree())); });
                }
            }

            CheckMatchesInProcess(currentToken);
        }

        if (_processMatches.Any())
            _processMatches.Clear();
    }

    private void CheckMatchesInProcess(Token currentToken)
    {
        var matchesToRemove = new List<Match>();
        foreach (var matchToCheckState in _processMatches)
        {
            var checkResult = matchToCheckState.CheckToken(currentToken);

            if (checkResult == MatchState.TokenMatch)
                continue;

            if (checkResult == MatchState.FullMatch)
            {
                if (NotIntersectWithExist(matchToCheckState))
                    Matches.Add(matchToCheckState);
            }

            matchesToRemove.Add(matchToCheckState);
        }

        if (matchesToRemove.Any())
        {
            matchesToRemove.ForEach(removeMatch => _processMatches.Remove(removeMatch));
        }
    }

    //проверять что не пересекаются
    private bool NotIntersectWithExist(Match matchToCheckState)
    {
        foreach (var match in Matches)
        {
            if (match.StartPosition <= matchToCheckState.StartPosition &&
                match.StartPosition + match.Lenght >= matchToCheckState.StartPosition)
            {
                return false;
            }
        }

        return true;
    }
}