using Markdown.Tokens;

namespace Markdown.Parser.MatchingTools;

public class MatchPattern
{
    public static PatternBody Start() => new();
    public static PatternBody StartWith(TokenType tokenType) => new(tokenType);

    public class PatternBody
    {
        private readonly List<TokenType> rawPattern = [];

        public PatternBody(TokenType tokenType)
            => rawPattern.Add(tokenType);

        public PatternBody()
        {
        }

        public PatternBody ContinueWith(TokenType nextType)
        {
            rawPattern.Add(nextType);
            return this;
        }

        public PatternBody ContinueWith(List<TokenType> nextTypes)
        {
            rawPattern.AddRange(nextTypes);
            return this;
        }

        public PatternBody ContinueWithRepeat(List<TokenType> repeatedTypes, int count)
        {
            for (var _ = 0; _ < count; _++)
                ContinueWith(repeatedTypes);
            return this;
        }

        public Pattern EndWith(TokenType endType)
        {
            rawPattern.Add(endType);
            return End();
        }
        
        public Pattern End() => new(rawPattern);
    }
}