using System.Collections.Generic;
using FluentAssertions;
using Markdown.Languages;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class LanguageTagDict_should
    {
        [Test]
        public void WhenUnregulatedTag_ReturnEmptyTag()
        {
            var dict = new LanguageTagDict(new Dictionary<TagType, Tag>());
            dict[TagType.Em].Start.Should().Be("");
            dict[TagType.Em].End.Should().Be("");
            dict[TagType.Em].Children.Should().NotContainNulls();
        }

        [Test]
        public void WhenRegulatedTag_ReturnEmptyTag()
        {
            var dict = new LanguageTagDict(new Dictionary<TagType, Tag>
                {{TagType.Em, new Tag("start", "end", new TagType[0])}});
            dict[TagType.Em].Start.Should().Be("start");
            dict[TagType.Em].End.Should().Be("end");
            dict[TagType.Em].Children.Should().HaveCount(0);
        }
    }
}