namespace Markdown.Data;

public static class TagNames {
    public const string Strong = "strong";
    public const string Em = "em";
    public const string Header = "h1";
    public const string List = "li";
}

public static class TagFactory
{
    private static Tag _bold => new(TagNames.Strong);
    private static Tag _italic => new(TagNames.Em);
    private static Tag _header => new(TagNames.Header);
    private static Tag _list => new(TagNames.List);

    public static Tag BuildTag(string mark)
    {
        switch (mark)
        {
            case Marks.Header:
                return _header;
            case Marks.Bold:
                return _bold;
            case Marks.Italic:
                return _italic;
            case Marks.List:
                return _list;
            default:
                throw new ArgumentException("Wrong mark!");
        }
    }
}