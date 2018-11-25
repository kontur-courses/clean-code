namespace Markdown.TokenEssences
{
    public class Token:IToken
    {
        public TypeToken TypeToken { get; }
        public string Value { get; }

        public Token(TypeToken typeToken, string value)
        {
            TypeToken = typeToken;
            Value = value;
        }
    }
}