namespace Markdown;

public enum NodeCheckResult : int
{
    NotSuccess = 0,
    Success = 1,
    SuccessToSelf = 2,
    End = 3
}