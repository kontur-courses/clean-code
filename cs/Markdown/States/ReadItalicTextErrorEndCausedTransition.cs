namespace Markdown.States;

public class ReadItalicTextErrorEndCausedTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process == ProcessState.ReadItalicText && state.IsEndOfLine();
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}