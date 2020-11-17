using FluentAssertions;
using Markdown;
using Markdown.Tags;
using Markdown.Tags.BoldTag;
using Markdown.Tags.HeaderTag;
using Markdown.Tags.ItalicTag;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownFilterTest
    {
        [Test]
        public void MarkDownFilter_ReturnEmptyDictOnEmptyTagArray()
        {
            MarkdownFilter.FilterTags(new Tag[0], 0).Should().BeEmpty();
        }

        [Test]
        public void MarkDownFilter_ReturnCorrectDictionaryOnDoubleItalicTag()
        {
            var result = MarkdownFilter.FilterTags(new Tag[]
            {
                new OpenItalicTag(0),
                new CloseItalicTag(5)
            }, 7);
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenItalicTag(0));
            result[5].Should().BeEquivalentTo(new CloseItalicTag(5));
        }
        
        [Test]
        public void MarkDownFilter_ReturnCorrectDictionaryOnDoubleBoldTag()
        {
            var result = MarkdownFilter.FilterTags(new Tag[]
            {
                new OpenBoldTag(0),
                new CloseBoldTag(5)
            }, 7);
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenBoldTag(0));
            result[5].Should().BeEquivalentTo(new CloseBoldTag(5));
        }

        [Test]
        public void MarkdownFilter_ShouldIgnoreBoldTagInItalic()
        {
            var result = MarkdownFilter.FilterTags(new Tag[]
            {
                new OpenItalicTag(0),
                new OpenBoldTag(5),
                new CloseBoldTag(8),
                new CloseItalicTag(10)
            }, 17);
            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenItalicTag(0));
            result[10].Should().BeEquivalentTo(new CloseItalicTag(10));
        }

        [Test]
        public void MarkdownFilter_ShouldIgnoreIntersectTags()
        {
            var result = MarkdownFilter.FilterTags(new Tag[]
            {
                new OpenBoldTag(0),
                new OpenItalicTag(15),
                new CloseBoldTag(35),
                new CloseItalicTag(40)
            }, 45);
            result.Should().BeEmpty();
        }
        
        [Test]
        public void MarkdownFilter_ShouldIgnoreIntersectTags2()
        {
            var result = MarkdownFilter.FilterTags(new Tag[]
            {
                new OpenItalicTag(0),
                new OpenBoldTag(15),
                new CloseItalicTag(35),
                new CloseBoldTag(40)
            }, 45);
            result.Should().BeEmpty();
        }

        [Test]
        public void MarkdownFilter_RemoveOnlyIntersectTags()
        {
            var result = MarkdownFilter.FilterTags(new Tag[]
            {
                new OpenHeaderTag(0),
                new OpenBoldTag(10),
                new OpenItalicTag(15),
                new CloseBoldTag(35),
                new CloseItalicTag(40),
                new CloseHeaderTag(45)
            }, 45);

            result.Should().HaveCount(2);
            result[0].Should().BeEquivalentTo(new OpenHeaderTag(0));
            result[45].Should().BeEquivalentTo(new CloseHeaderTag(45));

        }        
    }
}