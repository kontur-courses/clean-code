using System.Text;
using FluentAssertions;
using Markdown;

namespace MarkdownTests;

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
    [TestCase("__could be _intersected but__ not_")]
    public void Tokenize_ShouldReturnNotIntersectedTokens(string text)
    {
        var tokenizer = new MdTokenizer();
        
        var tokens = tokenizer.Tokenize(text);
        
        var pairs = Enumerable
            .Range(0, tokens.Count - 1)
            .Select(i => tokens[i + 1])
            .Zip(tokens)
            .Select(pair => (prev: pair.Second, next: pair.First));
        pairs.Should().OnlyContain(pair => pair.next.Begin - pair.prev.Begin == pair.prev.Length);
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