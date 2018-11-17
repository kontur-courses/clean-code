namespace Markdown.Md.PairedTagsHandlers
{
    public class OpenEmphasisHandler : PairedTagsHandler
    {
        public override void Handle(MdPairedTagsState state, int position, MdToken token)
        {
            if (token.Type != MdType.OpenEmphasis)
            {
                Successor?.Handle(state, position, token);

                return;
            }

            state.InEmphasis = true;
            state.OpeningTagsTokens.Push((position, token));
            Successor?.Handle(state, position, token);
        }
    }
}