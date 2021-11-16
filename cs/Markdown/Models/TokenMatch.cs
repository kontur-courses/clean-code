namespace Markdown.Models
{
    public class TokenMatch
    {
        public int Start { get; init; }
        public int Length { get; set; }
        public IToken Token { get; init; }
    }
}