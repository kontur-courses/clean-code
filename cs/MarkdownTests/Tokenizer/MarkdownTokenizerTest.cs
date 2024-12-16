using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Tokenizer;

[TestFixture]
public class MarkdownTokenizerTest
{
    [TestCase("Text with numbers 321")]
    [TestCase("Some not specific text")]
    [TestCase("Text with __markdown__ characters")]
    [TestCase("_A_ __lot__ of _characters_ in _markdown_\n")]
    public void MarkdownTokenizer_Tokenize_TransformAllTextToTokens(string markdown)
    {
        var tokenizer = new MarkdownTokenizer();
        
        var tokens = tokenizer.Tokenize(markdown);
        
        var totalLength = tokens.Sum(token => token.Length);
        totalLength.Should().Be(markdown.Length);
    }
    
    [TestCase("Text with numbers 321")]
    [TestCase("Some not specific text")]
    [TestCase("Text with __markdown__ characters")]
    [TestCase("_A_ __lot__ of _characters_ in _markdown_\n")]
    public void MarkdownTokenizer_Tokenize_TokensPresentInCorrectOrder(string markdown)
    {
        var tokenizer = new MarkdownTokenizer();
        var tokens = tokenizer.Tokenize(markdown);
        tokens.ToText().Should().Be(markdown);
    }
}