using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Bold;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParserTests
{
    public class TokensCollectionBuilder
    {
        private readonly List<Token> tokens = new List<Token>();

        public TokensCollectionBuilder Bold()
        {
            tokens.Add(new BoldToken(GetNextTokenPosition(), 2, "__"));
            return this;
        }

        public TokensCollectionBuilder Italic()
        {
            tokens.Add(new ItalicToken(GetNextTokenPosition(), 1, "_"));
            return this;
        }

        public TokensCollectionBuilder Text(string text)
        {
            tokens.Add(new TextToken(GetNextTokenPosition(), text.Length, text));
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

        public static implicit operator Token[](TokensCollectionBuilder collectionBuilder) => collectionBuilder.ToArray();
    }
}