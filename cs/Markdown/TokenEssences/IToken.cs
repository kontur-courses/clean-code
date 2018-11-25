namespace Markdown.TokenEssences
{
    public interface IToken
    {
        TypeToken TypeToken { get; }
        string Value { get; }
    }
}