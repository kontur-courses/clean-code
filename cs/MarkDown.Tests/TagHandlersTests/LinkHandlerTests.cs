using FluentAssertions;
using Markdown.TagHandlers;

namespace MarkDownTests.TagHandlersTests;

[TestFixture]
[TestOf(typeof(LinkHandler))]
public class LinkHandlerTests
{
    private TagHandlerTests testHandler = new(new LinkHandler());

    [Test]
    public void Should_Throw_WhenInputNull() => testHandler.Should_Throw_WhenInputNull();
    
    [Test]
    public void Should_ReturnDefault_WhenCannotTransform() => testHandler.Should_ReturnDefault_WhenCannotTransform(); 
    
    [TestCase("")]
    [TestCase("]()")]
    [TestCase("[()")]
    [TestCase("[])")]
    [TestCase("[](")]
    [TestCase("[[]()")]
    [TestCase("[]]()")]
    [TestCase("[](()")]
    public void Should_IgnoreInvalidTags(string text) => testHandler.Should_IgnoreInvalidTags(text);
    
    [TestCase("[]()", ExpectedResult = "<a href=\"\"></a>")]
    [TestCase("[[]]()", ExpectedResult = "<a href=\"\">[]</a>")]
    [TestCase("[(]()", ExpectedResult = "<a href=\"\">(</a>")]
    [TestCase("[abc]([)", ExpectedResult = "<a href=\"[\">abc</a>")]
    [TestCase("[abc](abc.html)", ExpectedResult = "<a href=\"abc.html\">abc</a>")]
    public string Should_TransformCorrect(string text) => testHandler.Should_TransformCorrect(text);
}