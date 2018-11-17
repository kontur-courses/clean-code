namespace Markdown.Md.PairedTagsHandlers
{
    public class OpenStrongEmphasisHandler : PairedTagsHandler
    {
        public override void Handle(MdPairedTagsState state, int position, MdToken token)
        {
            if (token.Type != MdType.OpenStrongEmphasis)
            {
                Successor?.Handle(state, position, token);

                return;
            }

            if (state.InEmphasis)
            {
                state.InvalidTokens.AddLast((position, token));
            }
            else
            {
                state.OpeningTagsTokens.Push((position, token));
            }

            Successor?.Handle(state, position, token);
        }
    }
}