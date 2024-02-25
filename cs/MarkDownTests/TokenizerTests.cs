using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkDownTests;

public class TokenizerTests
{
    [TestCaseSource(typeof(TokenizerTestsData), nameof(TokenizerTestsData.TestData))]
    public void Tokenizer_Tests(string input, List<Token> expected)
    {
        var tokenizer = new Tokenizer(input).Tokenize();
        tokenizer.ToList().Should().BeEquivalentTo(expected);
    }
}