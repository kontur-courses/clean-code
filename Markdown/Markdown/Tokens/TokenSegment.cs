namespace Markdown
{
    internal class TokenSegment
    {
        private readonly Tag tag;

        public int StartPosition { get; }
        public int EndPosition { get; }
        private int InnerLength => EndPosition - (StartPosition + tag.Start.Length);

        internal TokenSegment(TokenInfo first, TokenInfo? second = null)
        {
            tag = second is null 
                ? TagFactory.GetOrAddSingleTag(first.Token) 
                : TagFactory.GetOrAddSymmetricTag(first.Token);
            
            StartPosition = first.Position;
            EndPosition = second?.Position ?? first.Position;
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