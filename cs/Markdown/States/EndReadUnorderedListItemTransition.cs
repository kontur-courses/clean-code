using Markdown.Tokens;

namespace Markdown.States;

public class EndReadUnorderedListItemTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.UnorderedListItem && state.IsEndOfLine();
    }

    public override void Do(State state)
    {
        state.Parent = state.Parent.Parent;
        state.ProcessTo(ProcessState.EndReadUnorderedListItem);
        state.MoveNext();
    }
}