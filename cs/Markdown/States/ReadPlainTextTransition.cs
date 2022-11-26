namespace Markdown.States;

public class ReadPlainTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Process.IsOneOf(ProcessState.ReadParagraph, ProcessState.ReadHeader, ProcessState.ReadPlainText,
            ProcessState.ReadBoldText,
            ProcessState.EndReadItalicText, ProcessState.EndReadPlainText, ProcessState.EndReadBoldText);
    }

    public override void Do(State state)
    {
        state.ProcessTo(ProcessState.ReadPlainText);
        state.ReadInput();
    }
}

public static class TransitionExtensions
{
    public static void ReadInput(this State state)
    {
        var input = state.Input;
        if (input.Length == 2 && input[0] == '\\')
            input = input[1..2];
        state.ValueBuilder.Append(input);
        state.MoveNext();
    }
}