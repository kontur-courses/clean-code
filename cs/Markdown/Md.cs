using System.Text.RegularExpressions;

namespace Markdown;

public class Md
{
    private readonly List<Tag> _tags;

    private List<Match> _proccessMatches;

    public List<Match> Matches;

    public Md()
    {
        _tags = new List<Tag>();
        _proccessMatches = new List<Match>();
        Matches = new List<Match>();
    }

    public string Render(string text)
    {
        var reader = new TextReader(text);

        return string.Empty;
    }
}