using System.Collections;
using Markdown;

namespace MarkdownTests;

public class TokenizerTests
{
    [Test, TestCaseSource(nameof(TokenizeTestCases))]
    public void Tokenize_ReturnsTextToken_OnTextOnly(string input, IEnumerable<IToken> expected)
    {
        var tokenizer = new MdTokenizer(input, new ITag[]{new BoldTag(), new HeaderTag(), new ItalicTag()});

        tokenizer.Tokenize().Should().BeEquivalentTo(expected, x => x.Excluding(y => y.Value));
    }
    
    
    public static IEnumerable TokenizeTestCases
    {
        get
        {
            yield return new TestCaseData("abc", new MdTextToken[] { new("abc") });
            yield return new TestCaseData("_", new MdTagToken[] { new(new ItalicTag()) });
            yield return new TestCaseData("__", new MdTagToken[] { new(new BoldTag()) });
            yield return new TestCaseData("# ", new MdTagToken[] { new(new HeaderTag()) });
            yield return new TestCaseData("# ", new MdTagToken[] { new(new HeaderTag()) });
        }
    }
}