using System.Collections;
using Markdown;

namespace MarkdownTests;

public class TokenizerTests
{
    [Test, TestCaseSource(nameof(TokenizeTestCases))]
    public void Tokenize_ReturnsTextToken_OnTextOnly(string input, IEnumerable<IToken> expected)
    {
        var tokenizer = new MdTokenizer(input);

        tokenizer.Tokenize().Should().BeEquivalentTo(expected);
    }
    
    
    public static IEnumerable TokenizeTestCases
    {
        get
        {
            yield return new TestCaseData("abc", new MdTextToken[] { new("abc") });
            yield return new TestCaseData("_", new MdTagToken[] { new(new ItalicTag(), new NeighboursContext(null, null)) });
            yield return new TestCaseData("__", new MdTagToken[] { new(new BoldTag(), new NeighboursContext(null, null)) });
            yield return new TestCaseData("# ", new MdTagToken[] { new(new HeaderTag(), new NeighboursContext(null, null)) });
            yield return new TestCaseData("# ", new MdTagToken[] { new(new HeaderTag(), new NeighboursContext(null, null)) });
        }
    }
}