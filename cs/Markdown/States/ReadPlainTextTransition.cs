namespace Markdown.States;

public class ReadPlainTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsOneOf(ProcessState.ReadParagraph, ProcessState.ReadHeader, ProcessState.ReadPlainText, ProcessState.ReadBoldText,
            ProcessState.EndReadItalicText, ProcessState.EndReadPlainText, ProcessState.EndReadBoldText);
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.ReadPlainText);
        state.ValueBuilder.Append(state.Input);
        state.MoveNext();
    }
}