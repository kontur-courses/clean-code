namespace Markdown.States;

public class ReadItalicTextTransition : Transition
{
    public override bool When(State state)
    {
        return (state.Input == "_" && state.Process.IsStateForPlaceTextualToken()) ||
            state.Process == ProcessState.ReadItalicText;
    }

    public override void Do(State state)
    {
        if (state.Process != ProcessState.ReadItalicText)
        {
            var currentIndex = state.Index;
            var process = state.Process;
            state.UndoActions.Push(until =>
            {
                state.ProcessTo(process);
                state.ValueBuilder.Clear();
                state.IgnoredTransitions.Add((this, until));
                state.MoveTo(currentIndex);
            });
        }

        state.ProcessTo(ProcessState.ReadItalicText);
        state.ReadInput();
    }
}