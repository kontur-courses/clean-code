namespace Markdown
{
    public interface ITagData
    {
        public FormattingState State { get; }
        public TagBorder IncomingBorder { get; }
        public TagBorder OutgoingBorder { get; }
    }
}