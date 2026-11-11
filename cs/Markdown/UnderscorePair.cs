namespace Markdown
{
    public record UnderscorePair(Underscore Opening, Underscore Closing)
    {
        public int Start => Opening.Position;

        public int End => Closing.Position;

        public bool IsBreakingWord => !IsInSameWord && (Opening.IsInsideWord || Closing.IsInsideWord);

        private bool IsInSameWord => Opening.WordIndex == Closing.WordIndex;
    }
}
