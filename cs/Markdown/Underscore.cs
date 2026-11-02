namespace Markdown
{
    public record Underscore(int Position, int Length, bool CanOpen, bool CanClose, bool IsInsideWord, int WordIndex);
}
