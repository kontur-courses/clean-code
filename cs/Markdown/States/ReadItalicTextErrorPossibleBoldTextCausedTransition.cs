namespace Markdown.States;

public class ReadItalicTextErrorPossibleBoldTextCausedTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process == ProcessState.ReadItalicText && state.Input == "_" &&
            state.ValueBuilder.Length == 1;
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}