namespace Markdown
{
    public class SecondLevelToken
    {
        private string tokenValue;
        private SecondLevelTokenType levelTokenType;

        public SecondLevelToken(string tokenValue, SecondLevelTokenType levelTokenType)
        {
            this.tokenValue = tokenValue;
            this.levelTokenType = levelTokenType;
        }

        public string GetTokenValue()
        {
            return tokenValue;
        }

        public SecondLevelTokenType GetSecondTokenType()
        {
            return levelTokenType;
        }

        public void ChangeTokenType(SecondLevelTokenType newTokenType)
        {
            levelTokenType = newTokenType;
        }
    }
}