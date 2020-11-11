namespace Markdown.Models.Tags
{
    internal abstract class Tag
    {
        public virtual string Opening => string.Empty;
        public virtual string Closing => string.Empty;
    }
}
