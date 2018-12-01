using System.Collections.Generic;

namespace Markdown
{
    public class CompositeRule : IRule
    {
        private readonly IRule[] rules;

        public CompositeRule(IRule[] rules)
        {
            this.rules = rules;
        }

        public List<Token> Apply(List<Token> symbolsMap)
        {
            var copiedTokens = new List<Token>(symbolsMap);
            foreach (var rule in rules)
                copiedTokens = rule.Apply(copiedTokens);

            return copiedTokens;
        }
    }
}