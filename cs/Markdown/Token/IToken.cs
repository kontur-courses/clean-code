namespace Markdown
{
    public interface IToken<TTag>
    {
        public TTag Tag { get; }
        public TagState TagState { get; }
        public string Content { get; }
    }
}
