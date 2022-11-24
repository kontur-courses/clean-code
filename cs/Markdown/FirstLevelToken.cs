namespace Markdown
{
    public class FirstLevelToken
    {
        private string tokenValue;
        private FirstLevelTokenType levelTokenType;
        
        public FirstLevelToken(string tokenValue, FirstLevelTokenType levelTokenType)
        {
            this.tokenValue = tokenValue;
            this.levelTokenType = levelTokenType;
        }

        public string GetTokenValue()
        {
            return tokenValue;
        }

        public FirstLevelTokenType GetFirstTokenType()
        {
            return levelTokenType;
        }

        public void PutNewFirstLevelType(FirstLevelTokenType newFirstLevelType)
        {
            this.levelTokenType = newFirstLevelType;
        }
    }
}