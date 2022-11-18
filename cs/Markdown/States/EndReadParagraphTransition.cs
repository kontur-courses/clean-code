using Markdown.Tokens;

namespace Markdown.States;

public class EndReadParagraphTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsOneOf(ProcessState.EndReadPlainText, ProcessState.EndReadItalicText,
                ProcessState.EndReadBoldText) &&
            state.IsEndOfLine() && state.Parent.Type == TokenType.Paragraph;
    }

    public override void Do(State state)
    {
        state.Parent = state.Document;
        state.ProcessTo(ProcessState.EndReadParagraph);
    }
}