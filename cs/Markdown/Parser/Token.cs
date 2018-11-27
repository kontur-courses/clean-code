namespace Markdown.Md
{
    public class Token : IToken
    {
        public string Value { get; set; }

        public TokenPairType PairType { get; set; }

        public string Type { get; set; }

        public Token(string type, string value, TokenPairType pairType)
        {
            Type = type;
            Value = value;
            PairType = pairType;
        }

        public Token(string type, string value)
        {
            Type = type;
            Value = value;
            PairType = TokenPairType.NotPair;
        }
    }
}