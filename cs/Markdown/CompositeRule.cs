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
        public SortedList<int, Token> Apply(SortedList<int, Token> symbolsMap)
        {
            throw new System.NotImplementedException();
        }
    }
}