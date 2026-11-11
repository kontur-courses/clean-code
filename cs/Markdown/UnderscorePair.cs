namespace Markdown
{
    public record UnderscorePair(Underscore Opening, Underscore Closing)
    {
        public int Start => Opening.Position;

        public int End => Closing.Position;

        public bool IsInSameWord => Opening.WordIndex == Closing.WordIndex;

        public bool IsBreakingWord => !IsInSameWord && (Opening.IsInsideWord || Closing.IsInsideWord);
    }
}
