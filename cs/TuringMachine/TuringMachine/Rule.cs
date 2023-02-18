namespace Turing;

public struct Rule
{
    public readonly string OpeningState, ClosingState;
    public readonly char OpeningLetter, ClosingLetter;
    public readonly char Action;

    public Rule(string opState, string clState, char opLetter, char clLetter, char action)
    {
        OpeningState = opState;
        ClosingState = clState;
        OpeningLetter = opLetter;
        ClosingLetter = clLetter;
        Action = action;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Rule rule)
            return Equals(rule);
        return false;
    }

    private bool Equals(Rule other)
    {
        return OpeningLetter == other.OpeningLetter
               && ClosingLetter == other.ClosingLetter
               && OpeningState == other.OpeningState
               && ClosingState == other.ClosingState
               && Action == other.Action;
    }
}