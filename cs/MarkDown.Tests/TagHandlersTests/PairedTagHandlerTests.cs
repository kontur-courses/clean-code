using FluentAssertions;
using Markdown.TagHandlers;

namespace MarkDownTests.TagHandlersTests;

[TestFixture(TestOf = typeof(PairedTagHandler))]
public class PairedTagHandlerTests
{
    private TagHandlerTests testHandler = new(new PairedTagHandler("_", "<em>"));

    [Test]
    public void Should_Throw_WhenInputNull() => testHandler.Should_Throw_WhenInputNull();
    
    [Test]
    public void Should_ReturnDefault_WhenCannotTransform() => testHandler.Should_ReturnDefault_WhenCannotTransform(); 
    
    [TestCase("")]
    [TestCase("__")]
    [TestCase("_abc")]
    [TestCase("abc_")]
    [TestCase("_ abc_")]
    [TestCase("_abc _")]
    [TestCase("\\_abc_")]
    [TestCase("_abc\\_")]
    public void Should_IgnoreInvalidTags(string text) => testHandler.Should_IgnoreInvalidTags(text);

    [TestCase("_abc_", ExpectedResult = "<em>abc</em>")]
    [TestCase(@"_\\_", ExpectedResult = @"<em>\\</em>")]
    [TestCase(@"_\__", ExpectedResult = @"<em>\_</em>")]
    public string Should_TransformCorrect(string text) => testHandler.Should_TransformCorrect(text);
}