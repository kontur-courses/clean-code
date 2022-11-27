using Markdown.Tokens;

namespace Markdown.States;

public class EndReadUnorderedListTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.UnorderedList && state.Process == ProcessState.EndReadUnorderedListItem &&
            state.Input != "- ";
    }

    public override void Do(State state)
    {
        state.Parent = state.Parent.Parent;
        state.ProcessTo(ProcessState.EndReadUnorderedList);
    }
}