namespace Markdown.LocalMarkdown
{
    internal abstract class LocalMarkdownMaker
    {
        public string Line { get; protected set; }
        public int BeginIndex { get; protected set; }
        public int EndIndex { get; protected set; }
        public abstract void MakeSubstringMarkdown(MarkdownActionType[] actions);
    }
}