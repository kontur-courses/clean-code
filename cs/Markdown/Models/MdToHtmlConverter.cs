using System;
using System.Collections.Generic;
using Markdown.Models.Rules;

namespace Markdown.Models
{
    internal class MdToHtmlConverter
    {
        private readonly IEnumerable<IRule> rules;

        public MdToHtmlConverter(IEnumerable<IRule> rules)
        {
            this.rules = rules;
        }
        public string ConvertMany(IEnumerable<TaggedToken> tokens)
        {
            throw new NotImplementedException();
        }

        private string ConvertToken(TaggedToken token)
        {
            throw new NotImplementedException();
        }
    }
}
