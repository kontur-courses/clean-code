using FluentAssertions;
using MarkDown;
using NUnit.Framework;

namespace MarkDownTests
{
    public class TextPrerarerTests
    {
        [Test]
        public void PrepareText_OnText_ShouldSplitCorrectly()
        {
            var text = "Позвоночные являются млекопитающими.\r\nНо это неточно.";
            var expected = new string[] { "Позвоночные являются млекопитающими.", "Но это неточно." };
            var result = TextPreparer.PrepareText(text);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
