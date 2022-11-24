namespace Markdown;

public enum StateNodeType : int
{
    StartPosition = 0,
    Main = 1,
    End = 2,
    Lookbehind = 3,
    Lookahead = 4,
}