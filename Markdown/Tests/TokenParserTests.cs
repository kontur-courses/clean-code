using NUnit.Framework;
using FluentAssertions;
using System.Collections;
using Markdown.Nodes;
using Markdown.Tokens;
using Markdown.Transformers;

namespace Markdown.Tests;

[TestFixture]
public class TokenParserTests
{
    public static IEnumerable TestCases
    {
        get { throw new NotImplementedException(); }
    }


    [TestCaseSource(nameof(TestCases))]
    public void StringTokenizer_ReturnsExpectedArray(IReadOnlyList<Token> tokens, IReadOnlyList<Node> expected)
    {
        var result = TokenParser.Parse(tokens);

        result.Should().BeEqualTo(expected);
    }
}