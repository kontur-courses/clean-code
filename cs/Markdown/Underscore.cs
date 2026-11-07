namespace Markdown
{
    public record Underscore(
        int Position,
        int Length,
        bool CanOpen,
        bool CanClose,
        bool IsInsideWord,
        int WordIndex)
    {
        private const int SingleUnderscoreLength = 1;
        private const string SingleUnderscoreTagName = "em";
        private const string DoubleUnderscoreTagName = "strong";

        public bool IsSingle => Length == SingleUnderscoreLength;

        public string TagName => IsSingle ? SingleUnderscoreTagName : DoubleUnderscoreTagName;
    }
}
