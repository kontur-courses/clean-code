using Markdown.Tags;

namespace Markdown.Tokenizer.Tokens;

/// <summary>
/// Токен тега
/// </summary>
/// <param name="content">Содержание</param>
/// <param name="tag">Название тега</param>
public class TagToken : IToken
{
    public TagToken(ITag tag)
    {
        Tag = tag;
    }
    public string? TextContent { get; init; }
    public List<IToken> Children { get; init; } = new List<IToken>();

    public Dictionary<string, string> Attributes = new Dictionary<string, string>();

    public int Position { get; set; }

    public ITag Tag { get; init; }
    public override string ToString()
    {
        return $"{Tag.GetType()} {TextContent}";
    }
}