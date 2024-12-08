using FluentAssertions;
using FluentAssertions.Collections;
using Markdown.Token;

namespace MarkDownTest.Extension;

public static class TokenExtension
{
    public static void AssertTokensEqual(this IEnumerable<Token> actual, IEnumerable<Token> expected)
    {
        actual.Should().BeEquivalentTo(expected, 
            options => options.WithStrictOrdering());
    }
}