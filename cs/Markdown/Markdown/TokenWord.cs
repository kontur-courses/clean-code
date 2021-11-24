namespace Markdown
{
    public class TokenWord : IToken
    {
        public string Content
        { get; set; }

        public TokenWord(string word)
        {
            Content = word;
        }
    }
}
