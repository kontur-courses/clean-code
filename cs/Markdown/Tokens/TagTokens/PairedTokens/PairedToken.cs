namespace Markdown
{
    public abstract class PairedToken : TagToken
    {
        public bool IsEscaped { get; private set; }
        public bool IsClosing { get; private set; }
        protected abstract string MarkdownValue { get; }

        public override string GetValue()
        {
            if (IsEscaped)
                return MarkdownValue;

            return IsClosing ? $"</{HtmlValue}>" : $"<{HtmlValue}>";
        }

        public void Escape() => IsEscaped = true;

        public void Close() => IsClosing = true;
    }
}
