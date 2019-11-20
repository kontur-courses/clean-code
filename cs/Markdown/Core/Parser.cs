using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Rules;

namespace Markdown.Core
{
    class Parser
    {
        private readonly IEnumerable<IRule> rules;

        public Parser(IEnumerable<IRule> rules)
        {
            this.rules = rules;
        }


        public List<TagToken> ParseLine(string line)
        {
            var singleTagTokens = new SingleTagParser(rules).ParseLine(line);
            var doubleTagTokens = new DoubleTagParser(rules).Parse(line);
            return singleTagTokens.Concat(doubleTagTokens).ToList();
        }
    }
}