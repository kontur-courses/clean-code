using System.Collections;
using Markdown;

namespace MarkdownTests;

public class TokenizerTests
{
    [Test, TestCaseSource(nameof(TokenizeTestCases))]
    public void Tokenize_ReturnsTextToken_OnTextOnly(string input, IEnumerable<MdToken> expected)
    {
        var tokenizer = new MdTokenizer(input);

        tokenizer.Tokenize().Should().BeEquivalentTo(expected);
    }
    
    
    public static IEnumerable TokenizeTestCases
    {
        get
        {
            yield return new TestCaseData("abc", new MdToken[] { new("abc") });
            yield return new TestCaseData("_", new MdToken[] { new(new ItalicTag()) });
            yield return new TestCaseData("__", new MdToken[] { new(new BoldTag()) });
            yield return new TestCaseData("# ", new MdToken[] { new(new HeaderTag()) });
            yield return new TestCaseData("# ", new MdToken[] { new(new HeaderTag()) });
        }
    }
}