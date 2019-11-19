using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Rules;

namespace Markdown.Core
{
    static class Parser
    {
        public static List<TagToken> Parse(string line, IEnumerable<IRule> rules)
        {
            rules = rules.OrderByDescending(r => r.SourceTag.Opening.Length);
            var result = new List<TagToken>();
            var tokenStack = new Stack<TagToken>();
            for (var index = 0; index < line.Length; index++)
            {
                var currentRule = rules.FirstOrDefault(rule =>
                    rule.SourceTag.Opening.Length + index <= line.Length &&
                    rule.SourceTag.Opening == line.Substring(index, rule.SourceTag.Opening.Length));
                if (currentRule == null) continue;

                if (TagValidator.IsPossibleOpenningTag(line, index, currentRule.SourceTag))
                {
                    tokenStack.Push(new TagToken(index, currentRule.SourceTag, true));
                    index += currentRule.SourceTag.Opening.Length - 1;
                }
                else if (TagValidator.IsPossibleClosingTag(line, index, currentRule.SourceTag))
                {
                    while (tokenStack.Count > 0 && tokenStack.Peek().Tag != currentRule.SourceTag)
                        tokenStack.Pop();
                    if (tokenStack.Count == 0) continue;

                    result.Add(tokenStack.Pop());
                    result.Add(new TagToken(index, currentRule.SourceTag, false));
                    index += currentRule.SourceTag.Opening.Length - 1;
                }
            }

            return result;
        }
    }
}