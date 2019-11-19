using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags
{
    class MdPairTagBase : MdTagBase
    {
        private readonly IPairTagAndTokenComparer tagAndTokenComparer;

        public MdPairTagBase(Tag open, Tag close, IPairTagAndTokenComparer tagAndTokenComparer)
        {
            Open = open;
            Close = close;
            this.tagAndTokenComparer = tagAndTokenComparer;
        }

        public Tag Open { get; private set; }
        public Tag Close { get; private set; }
        public bool CloseTagIfNotFoundClosingTag { get => tagAndTokenComparer.CloseTagIfNotFoundClosingTag; }

        public bool IsTokenOpenTag(Token token) => tagAndTokenComparer.IsTokenOpenTag(token, Open);
        public bool IsTokenCloseTag(Token token) => tagAndTokenComparer.IsTokenCloseTag(token, Close);
    }
}