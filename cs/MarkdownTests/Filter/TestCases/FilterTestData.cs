using Markdown.Tokens;

namespace MarkdownTests.Filter.TestCases;

public class FilterTestData
{
    public string Line { get; }

    public List<Token> Tokens { get; }

    public List<Token> Expected { get; }

    public FilterTestData(string line, List<Token> tokens, List<Token> expected)
    {
        Line = line;
        Tokens = tokens;
        Expected = expected;
    }
}