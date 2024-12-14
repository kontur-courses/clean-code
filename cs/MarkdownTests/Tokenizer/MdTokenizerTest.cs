using System.Text;
using FluentAssertions;
using Markdown.Tokenizer;

namespace MarkdownTests.Tokenizer;

[TestFixture]
[TestOf(typeof(MdTokenizer))]
public class MdTokenizerTest
{
    private const string TextWithAllTokens = "#Text with all _possible_ __tokens__ 100% types\n";

    [TestCase(TextWithAllTokens)]
    public void Tokenize_ShouldTransformAllTextToTokens(string text)
    {
        var tokenizer = new MdTokenizer();
        
        var tokens = tokenizer.Tokenize(text);
        
        var totalLength = tokens.Sum(t => t.Length);
        totalLength.Should().Be(text.Length);
    }

    [TestCase(TextWithAllTokens)]
    public void Tokenize_ShouldReturnTokensInExpectedOrder(string text)
    {
        var tokenizer = new MdTokenizer();
        
        var tokens = tokenizer.Tokenize(text);
        var resultStringBuilder = tokens
            .Aggregate(new StringBuilder(), (sb, token) => sb.Append(token.Value));
        
        resultStringBuilder.ToString().Should().Be(text);
    }
}