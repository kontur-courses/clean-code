using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Models.Rules;

namespace Markdown.Models.Renders
{
    internal class Md
    {
        private readonly IEnumerable<IRule> rules;

        public Md(IEnumerable<IRule> rules)
        {
            this.rules = rules;
        }

        public string Render(string text)
        {
            var tokens = new TokenReader(rules.Select(rule => rule.From))
                .ReadTokens(text);

            return new MdToHtmlConverter(rules)
                .ConvertMany(tokens);
        }
    }
}
