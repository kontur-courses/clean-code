namespace Markdown
{
    public class SecondLevelToken
    {
        private string tokenValue;
        private FirstTokenType tokenType;
        
        public SecondLevelToken(string tokenValue, FirstTokenType tokenType)
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