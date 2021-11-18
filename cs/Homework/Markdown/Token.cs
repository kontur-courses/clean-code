namespace Markdown
{
    public abstract class Token
    {
        protected Token(TokenType type, string value, int startIndex, int finishIndex)
        {
            TokenType = type;
            Value = value;
            StartIndex = startIndex;
            FinishIndex = finishIndex;
        }

        public string Value { get; }
        public TokenType TokenType { get; }
        public int StartIndex { get; }
        public int FinishIndex { get; }
    }
}