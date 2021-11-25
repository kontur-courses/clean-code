using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenProvider
    {
        private readonly Dictionary<Type, Stack<Token>> stacks;

        private TokenProvider()
        {
            stacks = new Dictionary<Type, Stack<Token>>();
        }

        public static TokenProvider Create() => new TokenProvider();

        public TokenProvider InitTokenProvider(string src)
        {
            stacks.Add(typeof(ItalicsTag), new Stack<Token>());
            stacks.Add(typeof(BoldTag), new Stack<Token>());
            stacks.Add(typeof(HeadingTag), new Stack<Token>());
            stacks.Add(typeof(ShieldingTag), new Stack<Token>());
            stacks[typeof(HeadingTag)].Push(new Token(0, new HeadingTag()) {Length = src.Length - 1});
            return this;
        }

        public Stack<Token> GetStack(Type type) => stacks[type];

        public IEnumerable<Token> GetAllTokens()
        {
            return stacks.Values.SelectMany(s => s.ToArray());
        }
    }
}