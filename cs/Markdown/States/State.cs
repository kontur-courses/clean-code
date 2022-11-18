using System.Text;
using Markdown.Tokens;

namespace Markdown.States;

public class State
{
    private readonly string markdown;

    private readonly SortedSet<string> specialSequences = new()
    {
        "__",
        "# "
    };

    public State(string markdown)
    {
        this.markdown = markdown.ReplaceLineEndings("\n");
        Document = new();
        Parent = Document;
        ValueBuilder = new();
        Process = ProcessState.ReadDocument;
        MoveTo(0);
    }

    public DocumentToken Document { get; }

    public Token Parent { get; set; }

    public StringBuilder ValueBuilder { get; }

    public ProcessState Process { get; private set; }

    public string Input { get; private set; } = string.Empty;

    public int Index { get; private set; }

    public int InputLength { get; private set; }

    public bool EndOfFile { get; private set; }

    public HashSet<(Transition transition, int until)> IgnoredTransitions { get; } = new();

    public HashSet<(int index, string value)> IgnoredSpecialSequences { get; } = new();

    public Stack<Action<int>> UndoActions { get; } = new();

    public ReadOnlySpan<char> Markdown => markdown;

    public void ProcessTo(ProcessState processState)
    {
        Process = processState;
    }

    public void MoveNext()
    {
        MoveTo(Index + InputLength);
    }

    public void MoveTo(int index)
    {
        if (index >= Markdown.Length)
        {
            EndOfFile = true;
            return;
        }

        EndOfFile = false;

        IgnoredTransitions.RemoveWhere(x => x.until < index);
        IgnoredSpecialSequences.RemoveWhere(x => x.index < index);

        Index = index;
        var stateInputDoubled = GetString(Index, 2);
        InputLength = specialSequences.Contains(stateInputDoubled) &&
            !IgnoredSpecialSequences.Contains((index, stateInputDoubled))
                ? 2
                : 1;
        Input = GetString(Index, InputLength);
    }

    private string GetString(int index, int length)
    {
        if (index >= Markdown.Length)
            return string.Empty;

        var upperBound = index + length - 1;
        if (upperBound >= Markdown.Length && index < Markdown.Length)
            return Markdown[index..].ToString();

        var upperBoundExclusive = upperBound + 1;
        return Markdown[index..upperBoundExclusive].ToString();
    }

    public override string ToString()
    {
        return $@"Input:{Input};
Index:{Index};
Value:{ValueBuilder};
Process:{Process};
Parent:{Parent};
Document:{Document};
UndoActions:[{string.Join("; ", UndoActions)};
IgnoredTransition:[{string.Join("; ", IgnoredTransitions)}];
IgnoredSS:[{string.Join("; ", IgnoredSpecialSequences)}];";
    }
}