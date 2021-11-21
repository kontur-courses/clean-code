namespace Markdown
{
    public abstract class Tag
    {
        public abstract string OpenHTMLTag { get; }
        public abstract string CloseHTMLTag { get; }
        public abstract string OpenMdTag { get; }
        public abstract string CloseMdTag { get; }
        public abstract bool AllowNesting { get; }
    }
}
