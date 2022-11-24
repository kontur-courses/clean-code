namespace Markdown;

public class Tag
{
    private readonly PatternTree[] _patterns;
    private readonly string _name;

    public string Name => _name;
    public PatternTree[] Patterns => _patterns;


    public Tag(string name, PatternTree[] patterns)
    {
        _name = name;
        _patterns = patterns;
    }


    public List<PatternTree> CheckFirstCharMatch(Token token)
    {
        //проверяем, что первый символ подходит под наш паттерн и возвращаем дальнейшее дерево, из которого создадим Match
        var matches = new List<PatternTree>(_patterns.Length);
        foreach (var patternTree in _patterns)
        {
            if (patternTree.CheckFirstState(token) == MatchState.TokenMatch)
                matches.Add(patternTree.CopyPatternTree());
        }

        return matches;
    }
}