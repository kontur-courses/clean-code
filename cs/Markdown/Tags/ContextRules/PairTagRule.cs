namespace Markdown.Tags.ContextRules
{
    internal class PairTagRule : IContextRule
    {
        public virtual bool IsContextCorrect(ReadOnlySpan<char> context, int currentPosition, string tag) => true;

        public virtual bool IsContextEnd(ReadOnlySpan<char> context, int currentPosition, string tag)
        {
            return HaveFoundCloseTag(context, currentPosition, tag) 
                && IsLastContextSymbolNotSpace(context, currentPosition);
        }

        protected bool HaveFoundCloseTag(ReadOnlySpan<char> context, int currentPosition, string tag)
        {
            return currentPosition + tag.Length <= context.Length
                && context.Slice(currentPosition, tag.Length).ToString() == tag;
        }
        protected bool IsLastContextSymbolNotSpace(ReadOnlySpan<char> context, int currentPosition)
        {
            return currentPosition > 0 
                && context[currentPosition - 1] != ' ';
        }
    }
}
