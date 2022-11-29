using Markdown.Reading;

namespace Markdown.Parsing;

public class GeneralMdTag : IMdTag
{
    private readonly PatternTree[] _patterns;
    private readonly string _name;
    private readonly char? _escapeSymbol;
    private readonly bool _canBeParsed;

    public string Name => _name;
    public PatternTree[] Patterns => _patterns;

    public bool CanBeParsed => _canBeParsed;

    public GeneralMdTag(string name, PatternTree[] patterns, char? escapeSymbol = null, bool canBeParsed = false)
    {
        _name = name;
        _patterns = patterns;
        _escapeSymbol = escapeSymbol;
        _canBeParsed = canBeParsed;
    }


    public bool NeedEscape(Token token)
    {
        if (_escapeSymbol == null)
            return false;

        return token.Symbol == _escapeSymbol;
    }

    public virtual string ClearText(string text)
    {
        return text;
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