using Markdown.Tokens;

namespace Markdown.States;

public class ReadHeaderTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsOneOf(ProcessState.ReadDocument) && !state.IsEndOfLine() && state.Input == "# ";
    }

    public override void Do(State state)
    {
        var headerToken = new HeaderToken(state.Parent, string.Empty);
        state.ProcessTo(ProcessState.ReadHeader);
        state.MoveNext();
        state.Parent.AddChildren(headerToken);
        state.Parent = headerToken;
    }
}