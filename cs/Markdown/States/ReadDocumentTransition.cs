namespace Markdown.States;

public class ReadDocumentTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsOneOf(ProcessState.ReadDocument, ProcessState.EndReadParagraph,
                ProcessState.EndReadHeader) &&
            string.IsNullOrWhiteSpace(state.Input);
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.ReadDocument);
        state.MoveNext();
    }
}