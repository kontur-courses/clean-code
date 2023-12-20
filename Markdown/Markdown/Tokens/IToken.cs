namespace Markdown;

public interface IToken<out TTag>
{
    public TTag? Tag { get; }
    public string? Text { get; }
}