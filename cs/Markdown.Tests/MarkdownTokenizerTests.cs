using FluentAssertions;
using Markdown.Tokens;
using Markdown.Enums;

namespace MarkdownTests;

[TestFixture]
public class MarkdownTokenizerTests
{
    [Test]
    public void TransformTextToTokens_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MarkdownTokenizer.TransformTextToTokens(null));
    }

    [Test]
    public void TransformTextToTokens_EmptyInput_ReturnsSingleNewlineToken()
    {
        var inputText = "";
        var tokens = MarkdownTokenizer.TransformTextToTokens(inputText);

        tokens.Should().HaveCount(1)
            .And.ContainSingle(token => token.AssociatedTag.Type == TagType.Newline);
    }

    [Test]
    public void TransformTextToTokens_InputWithBoldTags_CreatesCorrectNumberOfTokens()
    {
        var inputText = "This is __bold__ text";

        var tokens = MarkdownTokenizer.TransformTextToTokens(inputText);

        tokens.Should().HaveCount(6);
    }
}