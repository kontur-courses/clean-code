namespace Markdown.Render;

public class TagReplaceRule : IReplaceRule
{
    private readonly string _startInsert;
    private readonly string _endInsert;

    public string TagId { get; }

    public TagReplaceRule(string tagId, string startInsert, string endInsert)
    {
        TagId = tagId;
        _startInsert = startInsert;
        _endInsert = endInsert;
    }

    public string ApplyRule(string text)
    {
        return $"{_startInsert}{text}{_endInsert}";
    }
}