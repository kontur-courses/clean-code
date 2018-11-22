using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TokenFlagLayouter
    {
        private readonly Dictionary<Func<char, bool>, Action<Token>> dependencies;

        public TokenFlagLayouter()
        {
            dependencies = new Dictionary<Func<char, bool>, Action<Token>>();
        }

        public void AddFlagDependency(Func<char, bool> dependency, Action<Token> flagSetter)
        {
            dependencies[dependency] = flagSetter;
        }

        public void LyaoutTags(char symbol, Token token)
        {
            foreach (var dependency in dependencies)
                if (dependency.Key(symbol))
                    dependency.Value(token);
        }
    }
}