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
            this.rules = rules.OrderByDescending(rule => rule.SourceTag.Opening.Length);
        }

        private bool IsSuitableRule(IRule rule, string line, int index) =>
            TagValidator.TagStartsFromPosition(line, index, rule.SourceTag.Opening) ||
            TagValidator.TagStartsFromPosition(line, index, rule.SourceTag.Closing);

        public List<TagToken> Parse(string line)
        {
            var result = new List<TagToken>();
            if (line == null)
                return result;

            var tagTokenStack = new Stack<TagToken>();
            for (var index = 0; index < line.Length; index++)
            {
                var currentRule = rules.FirstOrDefault(rule => IsSuitableRule(rule, line, index));
                if (currentRule == null) continue;

                if (TagValidator.IsPossibleOpeningTag(line, index, currentRule.SourceTag))
                {
                    tagTokenStack.Push(new TagToken(index, currentRule.SourceTag, true));
                }
                else if (TagValidator.IsPossibleClosingTag(line, index, currentRule.SourceTag))
                {
                    while (tagTokenStack.Count > 0 && tagTokenStack.Peek().Tag != currentRule.SourceTag)
                        tagTokenStack.Pop();
                    if (tagTokenStack.Count == 0) continue;

                    result.Add(tagTokenStack.Pop());
                    result.Add(new TagToken(index, currentRule.SourceTag, false));
                }

                index += currentRule.SourceTag.Opening.Length - 1;
            }

            return result;
        }
    }
}