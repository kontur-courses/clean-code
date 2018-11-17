namespace Markdown.Md.PairedTagsHandlers
{
    public class CloseEmphasisHandler : PairedTagsHandler
    {
        public override void Handle(MdPairedTagsState state, int position, MdToken token)
        {
            if (token.Type != MdType.CloseEmphasis)
            {
                Successor?.Handle(state, position, token);

                return;
            }

            state.InEmphasis = false;

            if (state.OpeningTagsTokens.Count != 0 && state.OpeningTagsTokens.Peek()
                .Item2.Type == MdType.OpenEmphasis)
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