namespace Markdown;

public class Tag
{
    private readonly PatternTree _pattern;

    private readonly IReplaceRule _replaceRule;

    public Tag(PatternTree pattern, IReplaceRule replaceRule)
    {
        _pattern = pattern;
        _replaceRule = replaceRule;
    }

    public PatternTree? CheckFirstCharMatch(Token token)
    {
        //проверяем, что первый символ подходит под наш паттерн и возвращаем дальнейшее дерево, из которого создадим Match

        return null;
    }
}