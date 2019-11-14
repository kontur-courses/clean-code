namespace Markdown.Core.Rules
{
    class RuleFactory
    {
        public static IRule[] CreateAllRules() => new IRule[] {new EmRule(), new StrongRule()};
    }
}