using System.Collections;
using Markdown;
using Markdown.Tags;
using Markdown.Tags.MdTags;
using Markdown.Tokens;

namespace MarkdownTests;

public class MdTokenizerTests
{
    [Test, TestCaseSource(nameof(TokenizeTestCases))]
    public void Tokenize_ReturnsCorrectTokens_OnText(string input, IEnumerable<IToken> expected)
    {
        var tokenizer = new MdTokenizer(input, new ITag[]{new BoldTag(), new HeaderTag(), new ItalicTag(), new UnorderedListTag()});

        tokenizer.Tokenize().Should().BeEquivalentTo(expected);
    }
    
    
    public static IEnumerable TokenizeTestCases
    {
        get
        {
            yield return new TestCaseData("abc", new IToken[] { new MdTextToken("abc"), new MdEndOfTextToken() });
            yield return new TestCaseData("_", new IToken[] { new MdTagToken(new ItalicTag()), new MdEndOfTextToken() });
            yield return new TestCaseData("__", new IToken[] { new MdTagToken(new BoldTag()), new MdEndOfTextToken() });
            yield return new TestCaseData("# ", new IToken[] { new MdTagToken(new HeaderTag()), new MdEndOfTextToken() });
            yield return new TestCaseData("\\", new IToken[] { new MdEscapeToken(), new MdEndOfTextToken() });
            yield return new TestCaseData("\n", new IToken[] { new MdNewlineToken(), new MdEndOfTextToken() });
            yield return new TestCaseData("* ", new IToken[] { new MdTagToken(new UnorderedListTag()), new MdEndOfTextToken() });
        }
    }
}