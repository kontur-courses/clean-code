using System.Collections.Generic;
using System.Linq;

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
            var correctTokens = symbolsMap.ToList();
            foreach (var rule in rules) correctTokens = rule.Apply(correctTokens);

            return correctTokens;
        }
    }
}