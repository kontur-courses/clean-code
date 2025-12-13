namespace Markdown.Parsers;

public readonly struct ParseStatus
{
    public ParseResultType Type { get; }

    private ParseStatus(ParseResultType type)
    {
        Type = type;
    }

    public static ParseStatus Ok()
    {
        return new ParseStatus(ParseResultType.Success);
    }

    public static ParseStatus Fail()
    {
        return new ParseStatus(ParseResultType.Fail);
    }

    public static ParseStatus WasRollback()
    {
        return new ParseStatus(ParseResultType.WasRollback);
    }

    public static ParseStatus NeedRollback()
    {
        return new ParseStatus(ParseResultType.NeedRollback);
    }
}

public enum ParseResultType
{
    Success,
    Fail,
    WasRollback,
    NeedRollback
}