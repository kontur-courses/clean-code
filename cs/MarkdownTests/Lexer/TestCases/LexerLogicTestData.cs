using System.Collections;
using Markdown.Tokens;

namespace MarkdownTests.Lexer.TestCases;

public class LexerLogicTestData
{
    public string Line { get; }

    public List<Token> Expected { get; }

    public LexerLogicTestData(string line, List<Token> expected)
    {
        Line = line;
        Expected = expected;
    }
}