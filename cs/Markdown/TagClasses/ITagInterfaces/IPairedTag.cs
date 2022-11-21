namespace Markdown.TagClasses.ITagInterfaces
{
    public interface IPairedTag : ITag
    {
        public bool CanBeStarter { get; set; }
        public bool CanBeEnder { get; set; }
        public bool InPair { get; set; }
        public bool InMiddle();
    }
}
