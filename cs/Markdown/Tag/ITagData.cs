namespace Markdown.Tag
{
    public interface ITagData
    {
        public TagBorder IncomingBorder { get; }
        public TagBorder OutgoingBorder { get; }
        public EndOfLineAction AtLineEndAction { get; }
        public bool IsBreaksWhenNestedNotComplete { get; }
        public ITagData ParentTag { get; }
        
        public bool IsValidAtOpen(string data, int startPos);
        public bool IsValidAtClose(string data, int startPos, int endPos);
        public bool CanNested(ITagData tagToNesting);
    }
}