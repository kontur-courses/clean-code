using Markdown.TextTokenizer;
using Markdown.Token;

namespace MarkdownTests;

public class TextTokenizerTests
{

    [TestCase("This is _italic_ and this is __bold__ text.", 8)]
    [TestCase("", 0)]
    [TestCase("This is plain text without any markup.", 1)]
    public void TextTokenizer_SplitsTextIntoTokens_Correctly(string markdownTest, int expectedCount)
    {
        ITextTokenizer textTokenizer = new TextTokenizer();
        
        List<Token> tokens = textTokenizer.Split(markdownTest);
        
        Assert.That(tokens.Count, Is.EqualTo(expectedCount));
    }
}