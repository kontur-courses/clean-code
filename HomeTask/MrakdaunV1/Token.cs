namespace MrakdaunV1
{
    public class Token
    {
        public TokenPart Part1;
        public TokenPart? Part2;

        public Token(TokenPart part1, TokenPart? part2 = null)
        {
            Part1 = part1;
            Part2 = part2;
        }
    }
}