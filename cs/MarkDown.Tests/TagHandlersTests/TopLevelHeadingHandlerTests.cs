using Markdown.TagHandlers;

namespace MarkDownTests.TagHandlersTests;

[TestFixture]
[TestOf(typeof(TopLevelHeadingHandler))]
public class TopLevelHeadingHandlerTests
{
    private TagHandlerTests testHandler = new(new TopLevelHeadingHandler());
    
    [Test]
    public void Should_Throw_WhenInputNull() => testHandler.Should_Throw_WhenInputNull();
    
    [Test]
    public void Should_ReturnDefault_WhenCannotTransform() => testHandler.Should_ReturnDefault_WhenCannotTransform(); 
    
    [TestCase("")]
    [TestCase("abc")]
    [TestCase("#abc")]
    [TestCase("##abc")]
    [TestCase(@"\# abc")]
    [TestCase("abc # abc")]
    public void Should_IgnoreInvalidTags(string text) => testHandler.Should_IgnoreInvalidTags(text);
    
    [TestCase(@"# \", ExpectedResult = @"<h1>\</h1>")]
    [TestCase("# abc", ExpectedResult = "<h1>abc</h1>")]
    [TestCase("# ab\n", ExpectedResult = "<h1>ab</h1>")]
    public string Should_TransformCorrect(string text) => testHandler.Should_TransformCorrect(text);
}