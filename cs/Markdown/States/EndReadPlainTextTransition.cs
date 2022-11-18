using Markdown.Tokens;

namespace Markdown.States;

public class EndReadPlainTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process == ProcessState.ReadPlainText && (state.Input.IsOneOf("_", "__") ||
            state.IsEndOfLine());
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.EndReadPlainText);
        var token = new TextToken(state.Parent, state.ValueBuilder.ToString());
        state.ValueBuilder.Clear();
        state.Parent.AddChildren(token);
    }
}