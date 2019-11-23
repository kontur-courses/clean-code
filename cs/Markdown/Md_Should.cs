using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("foo bar baz", "foo bar baz", Description = "Clear text")]
        [TestCase("foo _bar_ baz", "foo <em>bar</em> baz", Description = "Single underscores")]
        [TestCase("foo __bar__ baz", "foo <strong>bar</strong> baz", Description = "Double underscores")]
        [TestCase(@"foo \_bar\_ baz", "foo _bar_ baz", Description = "Escaped tags")]
        [TestCase("__foo _bar_ baz__", "<strong>foo <em>bar</em> baz</strong>",
            Description = "Single underscore inside double underscore")]
        [TestCase("_foo __bar__ baz_", "<em>foo __bar__ baz</em>",
            Description = "Double underscore inside single underscore")]
        [TestCase("foo_1_23", "foo_1_23", Description = "Underscores with digits")]
        [TestCase("__foo _bar", "__foo _bar", Description = "Characters without a pair")]
        [TestCase("_ foo", "_ foo", Description = "Whitespace after underscore")]
        [TestCase("_foo _", "_foo _", Description = "Whitespace before underscore")]
        [TestCase("__foo _bar__ baz_", "<strong>foo _bar</strong> baz_", Description = "Tags intersect")]
        public void Render_ReturnsCorrectString(string markdown, string expected)
        {
            Md.Render(markdown).Should().Be(expected);
        }
    }
}