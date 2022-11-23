namespace Markdown.TagClasses.ITagInterfaces
{
    internal interface ITextTag : ITag
    {
        public string Source { get; set; }
        public string Name { get; set; }
    }
}