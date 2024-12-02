using FluentAssertions;
using Markdown.Converters;
using Markdown.Models;

namespace Markdown.Tests.ConverterTests;

[TestFixture]
[TestOf(typeof(HeaderConverter))]
public partial class HeaderConverterTests
{
    private readonly HeaderConverter testHandler = new();

    [Test]
    [TestCaseSource(nameof(noConversionTestCases))]
    [TestCaseSource(nameof(conversionTestCases))]
    public void Convert_ShouldCorrectlyConvert_VariousInputs(string text, IEnumerable<TagPair> expectedTags)
    {
        var convertResult = testHandler.Convert(text);
        convertResult.Should().BeEquivalentTo(expectedTags);
    }
}