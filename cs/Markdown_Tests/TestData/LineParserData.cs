using Markdown.Tags;
using Markdown.Tags.ConcreteTags;

namespace MarkdownTests.TestData
{
    public static class LineParserData
    {
        public static IEnumerable<TestCaseData> WordsOnlyLines()
        {
            yield return new TestCaseData("word bubo bibo", "word bubo bibo", new List<ITag>());
            yield return new TestCaseData("Why1 word 23 bubo bibo", "Why1 word 23 bubo bibo", new List<ITag>());
        }

        public static IEnumerable<TestCaseData> LineWithHeader()
        {
            yield return new TestCaseData("# word bubo bibo", "word bubo bibo", new List<ITag>()
            {
                new HeaderTag(0, false),
                new HeaderTag(14, true),
            });
            yield return new TestCaseData("# Why1 word 23 bubo bibo", "Why1 word 23 bubo bibo", new List<ITag>()
            {
                new HeaderTag(0, false),
                new HeaderTag(22, true),
            });
        }

        public static IEnumerable<TestCaseData> LineWithItalic()
        {
            yield return new TestCaseData("_word bubo_ bibo", "word bubo bibo", new List<ITag>()
            {
                new ItalicTag(0, false),
                new ItalicTag(9, true),
            });
            yield return new TestCaseData("_word bu_bo bibo", "_word bu_bo bibo", new List<ITag>()
            {
            });
            yield return new TestCaseData("wo_rd bubo_ bibo", "wo_rd bubo_ bibo", new List<ITag>()
            {
            });
            yield return new TestCaseData("wo_rd bu_bo bibo", "wo_rd bu_bo bibo", new List<ITag>()
            {
            });
            yield return new TestCaseData("_wo_rd bubo bibo", "word bubo bibo", new List<ITag>()
            {
                new ItalicTag(0, false),
                new ItalicTag(2, true),
            });
            yield return new TestCaseData("_word __bubo__ bibo_", "word bubo bibo", new List<ITag>()
            {
                new ItalicTag(0, false),
                new BoldTag(5, false),
                new BoldTag(9, true),
                new ItalicTag(14, true),
            });
            yield return new TestCaseData("l_ov_e", "love", new List<ITag>()
            {
                new ItalicTag(1, false),
                new ItalicTag(3, true),
            });
            yield return new TestCaseData("l_ove_", "love", new List<ITag>()
            {
                new ItalicTag(1, false),
                new ItalicTag(4, true),
            });
            yield return new TestCaseData("_word __bubo_ bibo__", "_word __bubo_ bibo__", new List<ITag>());
            yield return new TestCaseData(@"\_word bubo_ bibo", "_word bubo_ bibo", new List<ITag>());
            yield return new TestCaseData(@"_word bubo\_ bibo", "_word bubo_ bibo", new List<ITag>());
            yield return new TestCaseData(@"\_word bubo\_ bibo", "_word bubo_ bibo", new List<ITag>());
            yield return new TestCaseData("Why_1_ word 23 bubo bibo", "Why_1_ word 23 bubo bibo", new List<ITag>());
        }

        public static IEnumerable<TestCaseData> LinesWithBulletedList()
        {
            yield return new TestCaseData("* один", "один", new List<ITag>()
            {
                new BulletTag(0, false),
                new BulletTag(4, true),
            });
            yield return new TestCaseData("* _один_ __два__", "один два", new List<ITag>()
            {
                new BulletTag(0, false),
                new ItalicTag(0, false),
                new ItalicTag(4, true),
                new BoldTag(5, false),
                new BoldTag(8, true),
                new BulletTag(8, true),
            });
            yield return new TestCaseData(@"\* один", "* один", new List<ITag>());
            yield return new TestCaseData(" * один", " * один", new List<ITag>());
            yield return new TestCaseData("горит * волбу", "горит * волбу", new List<ITag>());
        }

        public static IEnumerable<TestCaseData> LineWithBold()
        {
            yield return new TestCaseData("__word bubo__ bibo", "word bubo bibo", new List<ITag>()
            {
                new BoldTag(0, false),
                new BoldTag(9, true),
            });
            yield return new TestCaseData("__word bu__bo bibo", "__word bu__bo bibo", new List<ITag>()
            {
            });
            yield return new TestCaseData("wo__rd bubo__ bibo", "wo__rd bubo__ bibo", new List<ITag>()
            {
            });
            yield return new TestCaseData("wo__rd bu__bo bibo", "wo__rd bu__bo bibo", new List<ITag>()
            {
            });
            yield return new TestCaseData("__wo__rd bubo bibo", "word bubo bibo", new List<ITag>()
            {
                new BoldTag(0, false),
                new BoldTag(2, true),
            });
            yield return new TestCaseData("__word _bubo_ bibo__", "word bubo bibo", new List<ITag>()
            {
                new BoldTag(0, false),
                new ItalicTag(5, false),
                new ItalicTag(9, true),
                new BoldTag(14, true)
            });
            yield return new TestCaseData("l__ov__e", "love", new List<ITag>()
            {
                new BoldTag(1, false),
                new BoldTag(3, true),
            });
            yield return new TestCaseData("l__ove__", "love", new List<ITag>()
            {
                new BoldTag(1, false),
                new BoldTag(4, true),
            });
            yield return new TestCaseData("_word __bubo_ bibo__", "_word __bubo_ bibo__", new List<ITag>());
            yield return new TestCaseData(@"\__word bubo__ bibo", "__word bubo__ bibo", new List<ITag>());
            yield return new TestCaseData(@"__word bubo\__ bibo", "__word bubo__ bibo", new List<ITag>());
            yield return new TestCaseData(@"\__word bubo\__ bibo", @"__word bubo__ bibo", new List<ITag>());
            yield return new TestCaseData("Why__1__ word 23 bubo bibo", "Why__1__ word 23 bubo bibo", new List<ITag>());
        }

        public static IEnumerable<TestCaseData> MultiTagsLine()
        {
            //Это можно отнести к пересечению тегов
            yield return new TestCaseData("__word _bubo___", "__word _bubo___", new List<ITag>()
            {
            });
            //Это тоже по сути пересечение тегов
            yield return new TestCaseData("___word bubo___", "___word bubo___", new List<ITag>()
            {
            });
            yield return new TestCaseData("__word _bubo_ love__", "word bubo love", new List<ITag>()
            {
                new BoldTag(0, false),
                new ItalicTag(5, false),
                new ItalicTag(9, true),
                new BoldTag(14, true),
            });
            yield return new TestCaseData("___word_ bubo__", "word bubo", new List<ITag>()
            {
                new BoldTag(0, false),
                new ItalicTag(0, false),
                new ItalicTag(4, true),
                new BoldTag(9, true),
            });
            yield return new TestCaseData("__word _bu_ bo__", "word bu bo", new List<ITag>()
            {
                new BoldTag(0, false),
                new ItalicTag(5, false),
                new ItalicTag(7, true),
                new BoldTag(10, true),
            });
            yield return new TestCaseData("_word __bu__ bo_", "word bu bo", new List<ITag>()
            {
                new ItalicTag(0, false),
                new BoldTag(5, false),
                new BoldTag(7, true),
                new ItalicTag(10, true),
            });
        }
    }
}
