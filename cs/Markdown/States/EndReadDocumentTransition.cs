using Markdown.Tokens;

namespace Markdown.States;

public class EndReadDocumentTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsStateForPlaceContainerToken() && state.EndOfFile &&
            state.Parent.Type == TokenType.Document;
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.EndReadDocument);
    }
}