namespace Markdown.Tokenizer.Tokens;

/// <summary>
/// Текстовый токен. Содержит чистый текст
/// </summary>
/// <param name="content">Текст</param>
public class TextToken : IToken
{
    public TextToken(string content)
    {
        TextContent = content;
    }
    public string TextContent { get; init; }

    public List<IToken>? Children => null;
    
    public override string ToString()
    {
        return TextContent;
    }
}