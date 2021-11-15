namespace Markdown.Models
{
    public class TokenQueryMatch
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public IToken Token { get; set; }
    }
}