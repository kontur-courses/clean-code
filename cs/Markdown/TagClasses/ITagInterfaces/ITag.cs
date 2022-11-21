namespace Markdown.TagClasses.ITagInterfaces
{
    public interface ITag : IComparable
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public bool IsEscaped { get; set; }
        public TagType Type { get; set; }
        public string GetHtmlTag();
    }
}
