namespace Markdown.Types
{
    public class Token
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public  string Value { get; set; }
        public TypeToken TypeToken { get; set; }

        public Token(int position, int length, string value, TypeToken typeToken)
        {
            Position = position;
            Length = length;
            Value = value;
            TypeToken = typeToken;
        }
    }
}