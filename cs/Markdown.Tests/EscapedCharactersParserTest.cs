using FluentAssertions;
using Markdown.MarkdownParsers;

namespace Markdown.Tests;

[TestFixture]
public class EscapedCharactersParserTest
{
    [TestCase(@"a\_abc")]
    [TestCase(@"a\_ab\_c")]
    public void Parse_WithEscapedEscape_ReturnsCountMdTags(string text)
    {
        var result = new EscapedCharactersParser().Parse(text);

        result.Should().HaveCount(text.Count(x => x == '_'));
    }
}