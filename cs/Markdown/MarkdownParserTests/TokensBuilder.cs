using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParserTests
{
    public class TokensBuilder
    {
        private readonly List<Token> tokens = new List<Token>();

        public TokensBuilder Bold()
        {
            tokens.Add(Tokens.Bold(GetNextTokenPosition()));
            return this;
        }

        public TokensBuilder Italic()
        {
            tokens.Add(Tokens.Italic(GetNextTokenPosition()));
            return this;
        }

        public TokensBuilder Text(string text)
        {
            tokens.Add(Tokens.Text(GetNextTokenPosition(), text));
            return this;
        }

        public Token[] Range(int startIndex, int count)
        {
            return tokens.Skip(startIndex).Take(count).ToArray();
        }

        public Token At(int index)
        {
            if (index < 0)
                index = tokens.Count + index;
            return tokens[index];
        }

        public Token[] ToArray() => tokens.ToArray();

        public Token this[int index] => At(index);
        public Token[] this[int startIndex, int count] => Range(startIndex, count);

        private int GetNextTokenPosition()
        {
            if (tokens.Count == 0)
                return 0;
            var lastToken = tokens.Last();
            return lastToken.StartPosition + lastToken.RawLength - 1;
        }

        public static implicit operator Token[](TokensBuilder builder) => builder.ToArray();
    }
}