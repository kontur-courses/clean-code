namespace Markdown.Data;

public static class Marks
{
    public const string Bold = "__";
    public const string Italic = "_";
    public const string Header = "#";
    public const string List = "-";

    public static readonly IEnumerable<string> AllMarks = new[]
    {
        Bold,
        Italic,
        Header,
        List
    };

    public static string GetMarkByTagName(string name)
    {
        switch (name)
        {
            case TagNames.Header:
                return Header;
            case TagNames.Strong:
                return Bold;
            case TagNames.Em:
                return Italic;
            case TagNames.List:
                return List;
            default:
                throw new ArgumentException("Wrong name!");
        }
    }

    public static int AfterMarkSpace(string mark)
    {
        if (mark == Header || mark == List)
            return 1;
        return 0;
    }
}