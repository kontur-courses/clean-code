namespace Markdown;

public enum MatchState : int
{
    NotInitialized = 0,
    NotSuccess = 1,
    Process = 2,
    FullMatch = 3
}