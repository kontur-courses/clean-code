using System;

namespace Markdown.Tags
{
    public abstract class Tag
    {
        public abstract string OpenHtmlTag { get; }
        public abstract string CloseHtmlTag { get; }
        public abstract string OpenMdTag { get; }
        public abstract string CloseMdTag { get; }
        public abstract bool AllowNesting { get; }
        public abstract bool IsCorrectOpenTag(string mdText, int position);
        public abstract bool IsCorrectCloseTag(string mdText, int position);
    }
}
