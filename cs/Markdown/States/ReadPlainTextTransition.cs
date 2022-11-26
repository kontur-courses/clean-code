namespace Markdown.States;

public class ReadPlainTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsStateForPlaceTextualToken() || state.Process == ProcessState.ReadPlainText;
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.ReadPlainText);
        state.ReadInput();
    }
}