using System;
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


        public List<Token> Apply(List<Token> symbolsMap, List<TokenInformation> baseTokens)
        {
            throw new NotImplementedException();
        }
    }
}