namespace Markdown
{
    public class AnalyzedToken : Token
    {
        public AnalyzedTokenType TagType { get; private set; }

        public bool IsOpener => TagType == AnalyzedTokenType.Opener;
        public bool IsClosing => TagType == AnalyzedTokenType.Closing;
        public bool IsSentenceModifer => TagType == AnalyzedTokenType.SentenceModifer;

        public AnalyzedToken(int start, string value, TokenType type,
            AnalyzedTokenType analyzedTagType = AnalyzedTokenType.None)
            : base(start, value, type)
        {
            TagType = analyzedTagType;
        }

        public AnalyzedToken(Token token, AnalyzedTokenType tagType = AnalyzedTokenType.None)
            : base(token.Start, token.Value, token.Type)
        {
            TagType = tagType;
        }

        public void ChangeTagType(AnalyzedTokenType tagType)
        {
            TagType = tagType;
        }

        public new AnalyzedToken ToTextToken()
        {
            Type = TokenType.Text;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                AnalyzedToken analyzedToken => base.Equals(analyzedToken)
                                               && TagType == analyzedToken.TagType,

                Token token => base.Equals(token),

                _ => false
            };
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode() + TagType.GetHashCode() * 86243;
            }
        }
    }
}