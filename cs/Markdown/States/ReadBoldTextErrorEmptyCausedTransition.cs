using Markdown.Tokens;

namespace Markdown.States;

public class ReadBoldTextErrorEmptyCausedTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.Bold &&
            state.Input == "__" && !state.Parent.Children.Any()
            && state.ValueBuilder.Length == 0;
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}