namespace Markdown
{
    internal class TokenSegment
    {
        private readonly Tag tag;

        public TokenInfo StartTokenInfo { get; }
        public TokenInfo? EndTokenInfo { get; }

        public int StartPosition => StartTokenInfo.Position;
        public int EndPosition => EndTokenInfo?.Position ?? StartPosition;
        private int InnerLength => EndPosition - (StartPosition + tag.Start.Length);

        internal TokenSegment(TokenInfo first, TokenInfo? second = null)
        {
            tag = second is null 
                ? TagFactory.GetOrAddSingleTag(first.Token) 
                : TagFactory.GetOrAddSymmetricTag(first.Token);

            StartTokenInfo = first;
            EndTokenInfo = second;
        }
        
        public Tag GetBaseTag()
        {
            return tag;
        }

        public bool IsEmpty()
        {
            return InnerLength == 0;
        }
    }
}