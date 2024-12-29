namespace Markdown.Tags.ContextRules
{
    internal class PairTagSelectFewWordsRule : PairTagRule
    {
        public override bool IsContextEnd(ReadOnlySpan<char> context, int currentPosition, string tag)
        {
            return IsStringEndByTag(context, currentPosition, tag)
               || (base.IsContextEnd(context, currentPosition, tag) 
               && currentPosition + tag.Length < context.Length && !char.IsLetter(context[currentPosition + tag.Length]));
        }

        private bool IsStringEndByTag(ReadOnlySpan<char> context, int currentPosition, string tag)
        {
            return currentPosition + tag.Length == context.Length
                && context.Slice(currentPosition, tag.Length).ToString() == tag;
        }
    }
}
