using Markdown.Tokens;

namespace Markdown.States;

public class ReadBoldTextErrorAlfaNumericTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.Bold && state.Input.IsOneOf("__") &&
            state.ValueBuilder.Length != 0 &&
            state.ValueBuilder.AsEnumerable().All(char.IsDigit);
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}