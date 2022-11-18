using Markdown.Tokens;

namespace Markdown.States;

public class ReadBoldTextTransition : Transition
{
    public override bool When(State state)
    {
        return state.Input == "__" && state.Process.IsOneOf(ProcessState.ReadParagraph, ProcessState.ReadHeader, ProcessState.EndReadBoldText,
            ProcessState.EndReadItalicText, ProcessState.EndReadPlainText);
    }

    public override void Do(State state)
    {
        if (state.Process != ProcessState.ReadBoldText)
        {
            var currentIndex = state.Index;
            var process = state.Process;
            state.UndoActions.Push(until =>
            {
                state.ProcessTo(process);
                state.ValueBuilder.Clear();
                state.IgnoredTransitions.Add((this, until));
                state.IgnoredSpecialSequences.Add((currentIndex, "__"));
                state.Parent = state.Parent.Parent;
                state.Parent.RemoveChildren();
                state.MoveTo(currentIndex);
            });
        }

        var boldToken = new BoldTextToken(state.Parent, string.Empty);
        state.Parent.AddChildren(boldToken);
        state.Parent = boldToken;
        state.ProcessTo(ProcessState.ReadBoldText);
        state.MoveNext();
    }
}