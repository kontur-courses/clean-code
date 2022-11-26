using Markdown.Tokens;

namespace Markdown.States;

public class EndReadHeaderTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.Header
            && state.IsEndOfLine() &&
            state.Process.IsOneOf(ProcessState.ReadHeader, ProcessState.EndReadBoldText, ProcessState.EndReadItalicText,
                ProcessState.EndReadPlainText);
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.EndReadHeader);
        state.Parent = state.Parent.Parent;
    }
}