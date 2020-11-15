namespace Markdown.Tag
{
    public interface ITagData
    {
        public FormattingState State { get; }
        public TagBorder IncomingBorder { get; }
        public TagBorder OutgoingBorder { get; }

        public bool IsValid(string data, int startPos, int endPos);
        public bool CanNested(FormattingState stateToNesting);
    }
}