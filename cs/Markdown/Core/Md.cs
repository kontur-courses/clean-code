using Markdown.Core.Rules;

namespace Markdown.Core
{
    static class Md
    {
        public static string Render(string markdown)
        {
            var rules = RuleFactory.CreateAllRules();
            var tokens = Parser.Parse(markdown, rules);
            return Processor.Process(markdown, rules, tokens);
        }
    }
}