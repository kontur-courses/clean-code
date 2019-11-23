namespace Markdown.Core.Rules
{
    internal class RuleFactory
    {
        public static IRule[] CreateAllRules() => new IRule[]
        {
            new SingleUnderscoreToEm(), new DoubleUnderscoreToStrong(), new SharpToH1(), new ApostropheToCode(),
            new DoubleStarToStrong(), new StarToEm(), new GreaterToQ()
        };
    }
}