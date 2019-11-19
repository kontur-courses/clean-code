using Markdown.Core.Rules;

namespace Markdown.Core
{
    static class Md
    {
        public static string Render(string markdown)
        {
            var rules = RuleFactory.CreateAllRules();
            var tokens = new Parser(rules).Parse(markdown);
            return Processor.Process(markdown, rules, tokens);
        }
    }
}