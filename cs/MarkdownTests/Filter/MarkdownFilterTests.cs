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
    public void FilterTokens_RemovesIncorrectTags_WhenTagsAreInterruptedBySpaceSymbol()
    {
        var line = "_ a_  _a _";

        var tokens = new List<Token>
        {
            new(new EmphasisToken(), false, 0, 1),
            new(new EmphasisToken(), true, 3, 1),
            new(new EmphasisToken(), false, 6, 1),
            new(new EmphasisToken(), true, 9, 1)
        };

        var result = filter.FilterTokens(tokens, line);

        var expected = Enumerable.Empty<Token>();
        
        CollectionAssert.AreEqual(expected, result);
    }
    
    [Test]
    public void FilterTokens_RemovesIncorrectTags_WhenEmptyLineInBetweenPairedTags()
    {
        var line = "__str____str__ ____";

        var tokens = new List<Token>
        {
            new(new StrongToken(), false, 0, 2),
            new(new StrongToken(), true, 5, 2),
            new(new StrongToken(), false, 7, 2),
            new(new StrongToken(), true, 12, 2),
            new(new StrongToken(), false, 15, 2),
            new(new StrongToken(), true, 17, 2)
        };
        
        var result = filter.FilterTokens(tokens, line);

        var expected = new List<Token>
        {
            new(new StrongToken(), false, 0, 2),
            new(new StrongToken(), true, 5, 2),
            new(new StrongToken(), false, 7, 2),
            new(new StrongToken(), true, 12, 2)
        };
        
        CollectionAssert.AreEqual(expected, result);
    }
    
    [Test]
    public void FilterTokens_RemovesIncorrectTags_WhenTagsDoNotMatch()
    {
        var line = "__a_ __b_";

        var tokens = new List<Token>
        {
            new(new StrongToken(), false, 0, 2),
            new(new EmphasisToken(), false, 3, 1),
            new(new StrongToken(), true, 5, 2),
            new(new EmphasisToken(), true, 8, 1)
        };

        var result = filter.FilterTokens(tokens, line);

        var expected = Enumerable.Empty<Token>();
        
        CollectionAssert.AreEqual(expected, result);
    }
    
    [Test]
    public void FilterTokens_RemovesIncorrectTags_WhenTagsAreInDifferentWords()
    {
        var line = "_a aa_a ";

        var tokens = new List<Token>
        {
            new(new EmphasisToken(), false, 0, 1),
            new(new EmphasisToken(), true, 5, 1)
        };

        var result = filter.FilterTokens(tokens, line);
        
        var expected = Enumerable.Empty<Token>();
        
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void Tmp()
    {
        var line = "_a 1_3 a_";

        var tokens = new List<Token>
        {
            new(new EmphasisToken(), false, 0, 1),
            new(new EmphasisToken(), true, 4, 1),
            new(new EmphasisToken(), false, 8, 1)
        };

        var result = filter.FilterTokens(tokens, line);

        var expected = new List<Token>
        {
            new(new EmphasisToken(), false, 0, 1),
            new(new EmphasisToken(), true, 8, 1)
        };
    }
    
    [Test]
    public void FilterTokens_ReturnsCorrectResult_WithTextWithInnerTags()
    {
        var line = "_te_xt t_ex_t te_xt_ __123__ text_12_3";

        var tokens = new List<Token>
        {
            new(new EmphasisToken(), false, 0, 1),
            new(new EmphasisToken(), true, 3, 1),
            new(new EmphasisToken(), false, 8, 1),
            new(new EmphasisToken(), true, 11, 1),
            new(new EmphasisToken(), false, 16, 1),
            new(new EmphasisToken(), true, 19, 1),
            new(new StrongToken(), false, 21, 2),
            new(new StrongToken(), true, 26, 2),
            new(new EmphasisToken(), false, 33, 1),
            new(new EmphasisToken(), true, 36, 1)
        };

        var result = filter.FilterTokens(tokens, line);

        var expected = new List<Token>
        {
            new(new EmphasisToken(), false, 0, 1),
            new(new EmphasisToken(), true, 3, 1),
            new(new EmphasisToken(), false, 8, 1),
            new(new EmphasisToken(), true, 11, 1),
            new(new EmphasisToken(), false, 16, 1),
            new(new EmphasisToken(), true, 19, 1),
            new(new StrongToken(), false, 21, 2),
            new(new StrongToken(), true, 26, 2)
        };

        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void FilterTokens_ReturnsCorrectResult_WithNestedTags()
    {
        var line = "_e __s__ e_"; //__e _s_ e__

        var tokens = new List<Token>
        {
            new(new EmphasisToken(),false, 0, 1),
            new(new StrongToken(),false, 3, 2),
            new(new StrongToken(),true, 6, 2),
            new(new EmphasisToken(),true, 10, 1)
        };

        var result = filter.FilterTokens(tokens, line);

        var expected = new List<Token>
        {
            new(new EmphasisToken(),false, 0, 1),
            new(new EmphasisToken(),true, 10, 1)
        };

        CollectionAssert.AreEqual(expected, result);
    }
}