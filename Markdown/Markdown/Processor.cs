using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class Processor
    {
        public string Render(string source)
        {
            if (source == null)
                throw new ArgumentException();

            var tokens = ParseText(source);

            return ReplaceAttributesWithTags(tokens, source);
        }

        public IEnumerable<Token> ParseText(string source)
        {
            var tokens = FindPossibleTokens(source);
            tokens = RemoveNonPairDelimiters(tokens);
            tokens = MergeAdjacentDelimiters(tokens);

            return tokens;
        }

        public IEnumerable<Token> FindPossibleTokens(string source)
        {
            var possibleTokens = new List<Token>();

            for (var i = 0; i < source.Length; i++)
                if (Syntax.TypeDictionary.ContainsKey(source[i])
                    && Syntax.IsEscapeCharacter(source, i))
                    throw new NotImplementedException();

            return possibleTokens;
        }

        public IEnumerable<Token> RemoveNonPairDelimiters(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token> MergeAdjacentDelimiters(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        public string ReplaceAttributesWithTags(IEnumerable<Token> tokens, string source)
        {
            var index = 0;
            var sb = new StringBuilder();

            foreach (var token in tokens) throw new NotImplementedException();

            return sb.ToString();
        }
    }
}