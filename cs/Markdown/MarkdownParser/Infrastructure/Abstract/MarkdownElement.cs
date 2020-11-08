namespace MarkdownParser.Infrastructure.Abstract
{
    public abstract class MarkdownElement
    {
        protected MarkdownElement(int lastTokenIndex)
        {
            LastTokenIndex = lastTokenIndex;
        }

        public int LastTokenIndex { get; }
    }
}