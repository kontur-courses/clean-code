using Markdown.Tokens;

namespace Markdown.States;

public class ReadBoldTextErrorEndCausedTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.Bold && state.IsEndOfLine() &&
            state.Process.IsOneOf(ProcessState.ReadItalicText, ProcessState.ReadPlainText, ProcessState.ReadBoldText,
                ProcessState.EndReadItalicText, ProcessState.ReadPlainText);
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}