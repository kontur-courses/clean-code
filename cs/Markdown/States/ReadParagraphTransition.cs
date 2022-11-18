using Markdown.Tokens;

namespace Markdown.States;

public class ReadParagraphTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsOneOf(ProcessState.ReadDocument) &&
            state.Input.IsNotOneOf("\n", " ", "\0") && !state.IsEndOfLine();
    }

    public override void Do(State state)
    {
        var paragraphToken = new ParagraphToken(state.Parent, string.Empty);
        state.ProcessTo(ProcessState.ReadParagraph);
        state.Parent.AddChildren(paragraphToken);
        state.Parent = paragraphToken;
    }
}