namespace Markdown.States;

public class ReadItalicTextErrorInSeparateWordsTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process == ProcessState.ReadItalicText && state.Input.IsOneOf("_") &&
            state.ValueBuilder.Length > 1 && state.IsHighlightingInSeparateWords(1, state.ValueBuilder.Length);
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}