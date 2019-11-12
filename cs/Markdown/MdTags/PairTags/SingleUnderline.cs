using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;

namespace Markdown.MdTags.PairTags
{
    class SingleUnderline : MdPairTagBase
    {
        private readonly Tag open;
        private readonly Tag close;
        private readonly DefaultPairTagAndTokenComparer pairTagAndTokenComparer;

        public SingleUnderline()
        {
            open = new Tag() { Id = "open_", Value = "_" };
            close = new Tag() { Id = "close_", Value = "_" };
            pairTagAndTokenComparer = new DefaultPairTagAndTokenComparer(open, close);
        }

        public override Tag Open { get => open; }
        public override Tag Close { get => close; }

        public override bool IsTokenOpenTag(Token token) =>
            pairTagAndTokenComparer.IsTokenOpenTag(token);

        public override bool IsTokenCloseTag(Token token) =>
            pairTagAndTokenComparer.IsTokenCloseTag(token);
    }
}