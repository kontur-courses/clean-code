using Markdown.Tags;
using NUnit.Framework;

namespace Markdown_Tests
{
    public static class ParsedTextData
    {
        public static IEnumerable<TestCaseData> NullArguments()
        {
            yield return new TestCaseData(null, new List<ITag>()).SetName("Null_Paragraph");
            yield return new TestCaseData("", null).SetName("Null_Tags");
        }

        public static IEnumerable<TestCaseData> TagBeyondParagraph()
        {
            yield return new TestCaseData("", new List<ITag> { new ItalicTag(1) })
                .SetName("Empty_String_Tag_Position_Greater_Zero");
            yield return new TestCaseData("aboba", new List<ITag> { new HeaderTag(6) })
                .SetName("Tag_Position_Beyond_String");
        }

        public static IEnumerable<TestCaseData> TagNotOrderedByAscending()
        {
            yield return new TestCaseData("abracadabra", new List<ITag>
            {
                new ItalicTag(4), new ItalicTag(3),
                new ItalicTag(2), new ItalicTag(1)
            }).SetName("Tags_Ordered_By_Descending");
            yield return new TestCaseData("abracadabra", new List<ITag>
            {
                new ItalicTag(2), new ItalicTag(4),
                new ItalicTag(1), new ItalicTag(3)
            }).SetName("Tags_Not_Ordered");
        }
    }
}
