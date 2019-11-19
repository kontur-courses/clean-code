using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.Rules;

namespace Markdown.Core
{
    static class Processor
    {
        public static string Process(string text, IEnumerable<IRule> rules, IEnumerable<TagToken> tokens)
        {
            tokens = tokens.OrderBy(token => token.StartPosition);
            var renderedText = new StringBuilder(text);
            var offsetAfterAdding = 0;

            foreach (var token in tokens)
            {
                var currentRule = rules.FirstOrDefault(r => r.SourceTag == token.Tag);
                if (currentRule == null)
                    continue;
                var resultTag = currentRule.ResultTag;
                var tagValue = token.IsOpening ? resultTag.Opening : resultTag.Closing;

                renderedText.Remove(token.StartPosition + offsetAfterAdding, token.Length);
                renderedText.Insert(token.StartPosition + offsetAfterAdding, tagValue);

                offsetAfterAdding += tagValue.Length - token.Length;
            }

            return renderedText.ToString();
        }
    }
}