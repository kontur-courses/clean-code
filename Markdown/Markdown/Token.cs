namespace Markdown
{
    public class Token
    {
        public readonly Token[] nestedTokens;
        public readonly string type;
        public readonly string text;
        
        public Token(string type, string text, int startIndex , Token[] nestedTokens = null)
        {
            this.type = type;
            this.nestedTokens = nestedTokens;
        }
    }
}