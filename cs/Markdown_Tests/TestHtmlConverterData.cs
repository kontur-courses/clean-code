using Markdown.Tags;
using NUnit.Framework;

namespace Markdown_Tests
{
    public static class TestHtmlConverterData
    {
        public static IEnumerable<TestCaseData> TextNoTags()
        {
            yield return new TestCaseData("").SetName("Empty_String");
            yield return new TestCaseData("a").SetName("Short_String");
            yield return new TestCaseData(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt" +
                    " ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco " +
                    "laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in " +
                    "voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat " +
                    "non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                )
                .SetName("Long_String");
        }

        public static IEnumerable<TestCaseData> TagsWithText()
        {
            yield return new TestCaseData(
                "some text", new List<ITag> { new HeaderTag(0), new HeaderTag(9, true) },
                "<h>some text</h>"
            ).SetName("Header_Tag");
            yield return new TestCaseData(
                "some text", new List<ITag> { new ItalicTag(0), new ItalicTag(9, true) },
                "<em>some text</em>"
            ).SetName("Italic_Tag");
            yield return new TestCaseData(
                "some text", new List<ITag> { new BoldTag(0), new BoldTag(9, true) },
                "<strong>some text</strong>"
            ).SetName("Bold_Tag");
            yield return new TestCaseData(
                "some text", new List<ITag>
                {
                    new BoldTag(0), new ItalicTag(0),
                    new ItalicTag(9, true), new BoldTag(9, true)
                },
                "<strong><em>some text</em></strong>"
            ).SetName("Bold_And_Italic_Tags");
            yield return new TestCaseData(
                "some text", new List<ITag>
                {
                    new HeaderTag(0), new BoldTag(0), new ItalicTag(0),
                    new ItalicTag(9, true), new BoldTag(9, true),
                    new HeaderTag(9, true)
                },
                "<h><strong><em>some text</em></strong></h>"
            ).SetName("Header_Bold_And_Italic_Tags");
            yield return new TestCaseData(
                "text with some tags", new List<ITag>
                {
                    new ItalicTag(5), new ItalicTag(14, true)
                },
                "text <em>with some</em> tags"
            ).SetName("Italic_Tag_Inside_Text");
            yield return new TestCaseData(
                "text with some tags", new List<ITag>
                {
                    new BoldTag(5), new BoldTag(14, true)
                },
                "text <strong>with some</strong> tags"
            ).SetName("Bold_Tag_Inside_Text");
            yield return new TestCaseData(
                "text with some tags", new List<ITag>
                {
                    new HeaderTag(0), new BoldTag(0), 
                    new ItalicTag(0), new ItalicTag(4, true),
                    new ItalicTag(5), new ItalicTag(9, true),
                    new BoldTag(9, true), new BoldTag(10),
                    new BoldTag(14, true), new BoldTag(15),
                    new BoldTag(19, true), new HeaderTag(19, true)
                },
                "<h><strong><em>text</em> <em>with</em></strong> <strong>some</strong> <strong>tags</strong></h>"
            ).SetName("Multiple_Different_Tags_Inside_Text");
        }
    }
}
