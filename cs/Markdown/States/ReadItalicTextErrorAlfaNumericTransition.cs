namespace Markdown.States;

public class ReadItalicTextErrorAlfaNumericTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process == ProcessState.ReadItalicText && state.Input == "_" &&
            state.ValueBuilder.AsEnumerable().Skip(1).All(char.IsDigit);
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}