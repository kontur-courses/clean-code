namespace Markdown.Types
{
    public class Token
    {
        public TypeToken TypeToken { get; set; }
        public string Value { get; set; }

        public Token(TypeToken typeToken, string value)
        {
            TypeToken = typeToken;
            Value = value;
        }
    }
}