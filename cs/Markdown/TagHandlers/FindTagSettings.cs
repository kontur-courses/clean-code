namespace Markdown.TagHandlers;

public class FindTagSettings
{
    public bool SearchForHeading;
    public bool SearchForBold;
    public bool SearchForItalic;

    public FindTagSettings(bool heading, bool bold, bool italic)
    {
        SearchForHeading = heading;
        SearchForBold = bold;
        SearchForItalic = italic;
    }
}