using Markdown.Tokens;

namespace Markdown.States;

public class ReadUnorderedListTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.Document && state.Input == "- " &&
            state.Process.IsStateForPlaceContainerToken();
    }

    public override void Do(State state)
    {
        var unorderedList = new UnorderedListToken(state.Parent, string.Empty);
        state.Parent.AddChildren(unorderedList);
        state.Parent = unorderedList;
        state.ProcessTo(ProcessState.ReadUnorderedList);
    }
}