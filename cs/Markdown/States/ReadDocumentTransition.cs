namespace Markdown.States;

public class ReadDocumentTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsStateForPlaceContainerToken() && !state.EndOfFile;
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.ReadDocument);
        if (string.IsNullOrWhiteSpace(state.Input) || state.IsEndOfLine())
            state.MoveNext();
    }
}