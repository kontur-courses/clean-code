using FluentAssertions;
using Markdown.Converters;
using Markdown.Models;

namespace Markdown.Tests.ConverterTests;

[TestFixture]
[TestOf(typeof(BoldConverter))]
internal partial class BoldConverterTests
{
    private readonly BoldConverter testHandler = new();

    [Test]
    [TestCaseSource(nameof(noConversionTestCases))]
    [TestCaseSource(nameof(conversionTestCases))]
    public void Convert_ShouldCorrectlyConvert_VariousInputs(string text, IEnumerable<TagPair> expectedTags)
    {
        var convertResult = testHandler.Convert(text);
        convertResult.Should().BeEquivalentTo(expectedTags);
    }
}