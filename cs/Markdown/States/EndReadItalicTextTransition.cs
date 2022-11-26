using Markdown.Tokens;

namespace Markdown.States;

public class EndReadItalicTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process == ProcessState.ReadItalicText && state.Input == "_";
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.EndReadItalicText);
        var token = new ItalicTextToken(state.Parent, state.ValueBuilder.ToString(1, state.ValueBuilder.Length - 1));
        state.ValueBuilder.Clear();
        state.Parent.AddChildren(token);
        state.MoveNext();
        _ = state.UndoActions.Pop();
    }
}