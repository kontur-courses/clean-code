using FluentAssertions;
using Markdown;
using Markdown.Tokens;
using MdTest20.TestData;

namespace MdTest20;

public class ParserTests
{
    private readonly Dictionary<string, TagType> tagDictionary = new()
    {
        { "_", TagType.Italic },
        { "__", TagType.Bold },
        { "# ", TagType.Heading },
        { "* ", TagType.Bulleted },
        { "## ", TagType.Heading },
        { "### ", TagType.Heading }
    };

    [TestCaseSource(typeof(ParserTestData), nameof(ParserTestData.ConstructorParserExpectedTokenList))]
    public void ParseTests(string text, List<Token> expectedTokens)
    {
        var parser = new Parser(tagDictionary);
        parser.Parse(text).Should().BeEquivalentTo(expectedTokens);
    }
}