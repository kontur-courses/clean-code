namespace Markdown.Models
{
    public class TokenMatch
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public IToken Token { get; init; }
    }
}