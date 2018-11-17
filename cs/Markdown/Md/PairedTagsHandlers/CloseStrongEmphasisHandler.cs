namespace Markdown.Md.PairedTagsHandlers
{
    public class CloseStrongEmphasisHandler : PairedTagsHandler
    {
        public override void Handle(MdPairedTagsState state, int position, MdToken token)
        {
            if (token.Type != MdType.CloseStrongEmphasis)
            {
                Successor?.Handle(state, position, token);

                return;
            }

            if (state.InEmphasis)
            {
                state.InvalidTokens.AddLast((position, token));

                return;
            }

            if (state.OpeningTagsTokens.Count != 0 && state.OpeningTagsTokens.Peek()
                .Item2.Type == MdType.OpenStrongEmphasis)
            {
                state.OpeningTagsTokens.Pop();
            }
            else
            {
                state.InvalidTokens.AddLast((position, token));

                if (state.OpeningTagsTokens.Count != 0)
                {
                    state.InvalidTokens.AddLast(state.OpeningTagsTokens.Pop());
                }
            }

            Successor?.Handle(state, position, token);
        }
    }
}