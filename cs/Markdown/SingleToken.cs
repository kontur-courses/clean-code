namespace Markdown
{
    public class SingleToken
    {
        public SingleToken(TokenType tokenType, int tokenPosition, LocationType locationType)
        {
            TokenType = tokenType;
            TokenPosition = tokenPosition;
            LocationType = locationType;
        }

        public TokenType TokenType { get; }
        public int TokenPosition { get; }
        public LocationType LocationType { get; }
        public override string ToString()
        {
            return $"Type: {TokenType}, Position: {TokenPosition}, LocationType: {LocationType}";
        }
    }
}
