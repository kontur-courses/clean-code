using Markdown;

public class TagBuilder
{
    public static Tag Bold => new("strong");
    public static Tag Italic => new("em");
    public static Tag Header => new("h1");
    public static Tag List => new("ul");
    public static Tag ListItem => new("li");
    
    public TokenWrappers GetWrappers(string markdownTag)
    {
        switch (markdownTag)
        {
            case "Bold":
                return new TokenWrappers(Bold.OpeningTag, Bold.ClosingTag);
            case "Italic":
                return new TokenWrappers(Italic.OpeningTag, Italic.ClosingTag);
            case "Title":
                return new TokenWrappers(Header.OpeningTag, Header.ClosingTag);
            case "List":
                return new TokenWrappers(List.OpeningTag, List.ClosingTag);
            case "ListItem":
                return new TokenWrappers(ListItem.OpeningTag, ListItem.ClosingTag);
            default:
                throw new Exception($"Unknown tag '{markdownTag}'");
        }
    }
}