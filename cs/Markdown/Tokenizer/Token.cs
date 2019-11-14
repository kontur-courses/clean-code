namespace Markdown
{
    public class Token
    {
        public readonly string Text;
        public readonly TagType Type;
        public readonly int StartIndex;
        public readonly int EndIndex;

        public int Length => EndIndex - StartIndex + 1;

        public Token(string tokenText, TagType currentType, int startIndex, int endIndex)
        {
            Text = tokenText;
            Type = currentType;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public override string ToString()
        {
            return $"Token: Type {Type}, in text {StartIndex} - {EndIndex}, text part = \"{Text}\",";
        }
    }
}