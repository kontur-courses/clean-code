using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;

namespace Markdown.MdTags.PairTags
{
    class DoubleUnderline : MdPairTagBase
    {
        private readonly Tag open;
        private readonly Tag close;
        private readonly DefaultPairTagAndTokenComparer pairTagAndTokenComparer;

        public DoubleUnderline()
        {
            open = new Tag() { Id = "open__", Value = "__" };
            close = new Tag() { Id = "close__", Value = "__" };
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