using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class TokenConverterTests
{
    [TestCase("# Заголовок __с _разными_ символами__\na", 
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>\na")]
    [TestCase("__a _b_ a__", "<strong>a <em>b</em> a</strong>")]
    [TestCase("_a __b__ a_", "<em>a __b__ a</em>")]
    [TestCase("_a __b_ a__", "_a __b_ a__")]
    [TestCase("_a _ a_a__ 5_a_", "<em>a _ a_a__ 5_a</em>")]
    [TestCase(@"\_a_ \\\", @"_a_ \")]
    [TestCase("[This link](http://example.net/) has no title attribute.", 
        "<a href=\"http://example.net/\">This link</a> has no title attribute.")]
    [TestCase("This is [an example](http://example.com/ \"Title\") inline link.", 
        "This is <a href=\"http://example.com/\" title=\"Title\">an example</a> inline link.")]
    public void TokensToString(string input, string output)
    {
        var tokens = new TokenHighlighter().HighlightTokens(input);

        new TokenConverter().TokensToString(tokens).Should().BeEquivalentTo(output);
    }
}