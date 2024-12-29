namespace Markdown.Tags.ContextRules
{
    internal class PairTagSelectPartWordRule : PairTagRule
    {
        public override bool IsContextCorrect(ReadOnlySpan<char> context, int currentPosition, string tag)
        {
            return base.IsContextCorrect(context, currentPosition, tag) && char.IsLetter(context[currentPosition]);
        }
    }
}
