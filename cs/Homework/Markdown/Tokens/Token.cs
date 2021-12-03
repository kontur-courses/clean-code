namespace Markdown.Tokens
{
    public abstract class Token
    {
        protected Token(string value, string selector, int paragraphIndex, int startIndex)
        {
            Value = value;
            Selector = selector;
            ParagraphIndex = paragraphIndex;
            StartIndex = startIndex;
        }

        public string Value { get; }
        public int ParagraphIndex { get; }
        public int StartIndex { get; }
        public string Selector { get; }
        public int Length => Value.Length;
        public int FinishIndex => StartIndex + Length - 1;
    }
}