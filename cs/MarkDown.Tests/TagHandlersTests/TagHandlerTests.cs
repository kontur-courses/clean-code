using FluentAssertions;
using Markdown;
using Markdown.TagHandlers;

namespace MarkDownTests.TagHandlersTests;

public class TagHandlerTests
{
    private ITagHandler sut;

    public TagHandlerTests(ITagHandler sut)
    {
        this.sut = sut;
    }
    
    public void Should_Throw_WhenInputNull()
    {
        Action action = () => sut.Transform(null);

        action.Should().Throw<ArgumentNullException>();
    }

    public void Should_ReturnDefault_WhenCannotTransform()
    {
        var text = "abcdefg";
        
        var actual = sut.Transform(text);

        actual.Should().BeEquivalentTo(StringManipulator.Default(text));
    }
    
    public void Should_IgnoreInvalidTags(string text) => sut.Transform(text).Content.Should().Be(text);
    
    public string Should_TransformCorrect(string text)
    {
        return sut.Transform(text).Content;
    }
}