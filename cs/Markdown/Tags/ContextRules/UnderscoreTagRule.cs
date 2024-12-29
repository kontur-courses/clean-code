namespace Markdown.Tags.ContextRules
{
    internal class UnderscoreTagRule : IContextRule
    {
        public bool IsContextCorrect(ReadOnlySpan<char> context, int currentPosition, string tag)
        {
            return !char.IsDigit(context[currentPosition]) 
                && IsFirstContextSymbolNotSpace(context);
        }

        private bool IsFirstContextSymbolNotSpace(ReadOnlySpan<char> context)
        {
            return context.Length > 0 && context[0] != ' ';
        }
    }
}
