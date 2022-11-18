using Markdown.Tokens;

namespace Markdown.States;

public class ReadBoldTextErrorInSeparateWordsTransition : Transition
{
    public override bool When(State state)
    {
        return state.Parent.Type == TokenType.Bold &&
            state.Process.IsOneOf(ProcessState.EndReadItalicText, ProcessState.EndReadPlainText) &&
            state.Input.IsOneOf("__") &&
            state.IsHighlightingInSeparateWords(2,
                state.Parent.Children.Sum(x => x.Type == TokenType.Italic ? x.Value.Length + 2 : x.Value.Length));
    }

    public override void Do(State state)
    {
        state.UndoActions.Pop()(state.Index);
    }
}