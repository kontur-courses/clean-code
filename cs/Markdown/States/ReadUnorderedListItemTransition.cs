using Markdown.Tokens;

namespace Markdown.States;

public class ReadUnorderedListItemTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.UnorderedList && state.Input == "- " &&
            state.Process is ProcessState.EndReadUnorderedListItem or ProcessState.ReadUnorderedList;
    }

    public override void Do(State state)
    {
        var unorderedListItem = new UnorderedListItemToken(state.Parent, string.Empty);
        state.Parent.AddChildren(unorderedListItem);
        state.Parent = unorderedListItem;
        state.ProcessTo(ProcessState.ReadUnorderedListItem);
        state.MoveNext();
    }
}