using FluentAssertions;
using Markdown.Tokens;
using Markdown.Transformers;
using NUnit.Framework;
using System.Collections;

namespace Markdown.Tests;

[TestFixture]
public class MarkdownTests
{
    public static IEnumerable StringTokenizerTestCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("Hello World!", [Token.Text("Hello World!")]);
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# Hello World!",
                [Token.HeaderMarker(), Token.Text("Hello World!")]);
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_Hello World!_",
                [Token.Underscore(true, false), Token.Text("Hello World!"), Token.Underscore(false, true)]);
        }
    }

    
    [TestCaseSource(nameof(StringTokenizerTestCases))]
    public void StringTokenizer_ReturnsExpectedArray(string markdown, IReadOnlyList<Token> expected)
    {
        var result = StringTokenizer.Tokenize(markdown);

        result.Should().Equal(expected);
    }
}