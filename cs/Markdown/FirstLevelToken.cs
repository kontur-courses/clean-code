namespace Markdown
{
    public class FirstLevelToken
    {
        private string tokenValue;
        private FirstTokenType tokenType;
        
        public FirstLevelToken(string tokenValue, FirstTokenType tokenType)
        {
            this.tokenValue = tokenValue;
            this.tokenType = tokenType;
        }

        public string GetTokenValue()
        {
            return tokenValue;
        }

        public FirstTokenType FirstTokenType()
        {
            return tokenType;
        }
    }
}