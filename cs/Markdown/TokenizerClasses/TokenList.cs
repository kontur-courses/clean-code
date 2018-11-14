using System.Collections.Generic;
using System.Linq;

namespace Markdown.TokenizerClasses
{
    public class TokenList
    {
        public List<Token> Tokens { get; }

        public TokenList(IEnumerable<Token> tokens)
        {
            Tokens = tokens.ToList();
        }

        public bool Peek(params string[] types)
        {
            if (Tokens.Count == 0 || types.Length > Tokens.Count)
                return false;

            return !types.Where((t, i) => Tokens[i].Type != t).Any();
        }

        public TokenList Offset(int offset)
        {
            if (offset == 0)
                return this;

            return new TokenList(Tokens.GetRange(offset, Tokens.Count - offset));
        }

        public Token GetFirst()
        {
            return Tokens[0];
        }

        public Token GetSecond()
        {
            return Tokens[1];
        }

        public Token GetThird()
        {
            return Tokens[2];
        }
    }
}
