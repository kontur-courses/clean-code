using Markdown.Filter;
using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace MarkdownTests.Filter;

[TestFixture]
public class MarkdownFilterTests
{
    private MarkdownFilter filter = null!;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        filter = new MarkdownFilter();
    }
    
    [Test]
    public void FilterTokens_RemovesInvalidTokens_WhenTokenIsEscaped()
    {
        var line = @"\_a\_";
        var inputList = new List<Token>
        {
            new(new EmphasisToken(), false, 1, 1),
            new(new EmphasisToken(), true, 5, 1)
        };

        var result = filter.FilterTokens(inputList, line);
        var expected = Enumerable.Empty<Token>();
        
        CollectionAssert.AreEqual(expected, result);
    }
}