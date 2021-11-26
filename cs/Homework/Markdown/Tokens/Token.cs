namespace Markdown.Tokens
{
    public abstract class Token
    {
        protected Token(string value, int paragraphIndex, int startIndex)
        {
            Value = value;
            ParagraphIndex = paragraphIndex;
            StartIndex = startIndex;
        }

        public string Value { get; }
        public int ParagraphIndex { get; }
        public int StartIndex { get; }
        public int Length => Value.Length;
    }
}