using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.HTMLTransducer;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            var tokens = new Tokenizer()
                .GetTokens(text, new List<string>{"_", "__"})
                .ToList();
            
            var rules = new Rules(new List<Rule>
            {
                new Rule(new Token("_", true), "<em>", doubleTagged: true),
                new Rule(new Token("__", true), "<strong>", doubleTagged: true)
            });
            var transducer = new Transducer();
            tokens = transducer.Transform(tokens, rules);

            return string.Join("", tokens.Select(t => t.Value));
        }
    }
}
