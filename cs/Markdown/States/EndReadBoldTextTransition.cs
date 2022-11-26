using Markdown.Tokens;

namespace Markdown.States;

public class EndReadBoldTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Input == "__" &&
            state.Parent.Type == TokenType.Bold &&
            state.Process.IsOneOf(ProcessState.EndReadPlainText, ProcessState.EndReadItalicText);
    }

    public override void Do(State state)
    {
        state.Parent = state.Parent.Parent;
        state.ProcessTo(ProcessState.EndReadBoldText);
        state.MoveNext();
    }
}