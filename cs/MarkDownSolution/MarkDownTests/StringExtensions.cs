using FluentAssertions;
using MarkDown;

namespace MarkDownTests
{
    public static class StringExtensions
    {
        public static void AfterProcessingShouldBe(this string input, string expected)
        {
            var text = input;
            var token = Tokenizer.GetToken(text);
            var actualOutput = HtmlTagger.GetString(token, text);

            var expectedOutput = expected;

            actualOutput.Should().Be(expectedOutput);
        }
    }
}
