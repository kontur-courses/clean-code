using Markdown.Tags;
using Markdown.Tokens;

namespace MarkdownTests.TestData;

public static class TokenGeneratorTestsData
{
    public static IEnumerable<TestCaseData> TextOnlyLines()
    {
        yield return new TestCaseData("вася", new List<Token>
        {
            new(TokenType.Text, "вася", 0)
        });

        yield return new TestCaseData("петя1223d", new List<Token>
        {
            new(TokenType.Text, "петя", 0),
            new(TokenType.Number, "1223", 4),
            new(TokenType.Text, "d",8)
        });

        yield return new TestCaseData("1234566", new List<Token>
        {
            new(TokenType.Number, "1234566", 0)
        });
    }

    public static IEnumerable<TestCaseData> WhiteSpacesOnlyLines()
    {
        yield return new TestCaseData(" ", new List<Token>
        {
            new(TokenType.WhiteSpace, " ", 0)
        });

        yield return new TestCaseData("  ", new List<Token>
        {
            new(TokenType.WhiteSpace, " ", 0),
            new(TokenType.WhiteSpace, " ", 1)
        });

        yield return new TestCaseData("      ", new List<Token>
        {
            new(TokenType.WhiteSpace, " ", 0),
            new(TokenType.WhiteSpace, " ", 1),
            new(TokenType.WhiteSpace, " ", 2),
            new(TokenType.WhiteSpace, " ", 3),
            new(TokenType.WhiteSpace, " ", 4),
            new(TokenType.WhiteSpace, " ", 5)
        });
    }

    public static IEnumerable<TestCaseData> LinesWithHeader()
    {
        yield return new TestCaseData("# Заголовок", new List<Token>
        {
            new(TokenType.MdTag, "# ", 0, false,TagType.Header),
            new(TokenType.Text, "Заголовок", 2)
        });

        yield return new TestCaseData("# # # ", new List<Token>
        {
            new(TokenType.MdTag, "# ", 0, false, TagType.Header),
            new(TokenType.MdTag, "# ", 2, false, TagType.Header),
            new(TokenType.MdTag, "# ", 4, false, TagType.Header)
        });

        yield return new TestCaseData(@" # # ", new List<Token>
        {
            new(TokenType.WhiteSpace, " ", 0),
            new(TokenType.MdTag, "# ", 1, false, TagType.Header),
            new(TokenType.MdTag, "# ", 3, false, TagType.Header)
        });
    }

    public static IEnumerable<TestCaseData> LinesWithBulletedList()
    {
        yield return new TestCaseData("* Заголовок", new List<Token>
        {
            new(TokenType.MdTag, "* ", 0, false, TagType.BulletedListItem),
            new(TokenType.Text, "Заголовок", 2)
        });

        yield return new TestCaseData("* * * ", new List<Token>
        {
            new(TokenType.MdTag, "* ", 0, false, TagType.BulletedListItem),
            new(TokenType.MdTag, "* ", 2, false, TagType.BulletedListItem),
            new(TokenType.MdTag, "* ", 4, false, TagType.BulletedListItem)
        });

        yield return new TestCaseData(@" * * ", new List<Token>
        {
            new(TokenType.WhiteSpace, " ", 0),
            new(TokenType.MdTag, "* ", 1, false, TagType.BulletedListItem),
            new(TokenType.MdTag, "* ", 3, false, TagType.BulletedListItem)
        });

        yield return new TestCaseData(@"* раз * два", new List<Token>
        {
            new(TokenType.MdTag, "* ", 0, false, TagType.BulletedListItem),
            new(TokenType.Text, "раз", 2),
            new(TokenType.WhiteSpace, " ", 5),
            new(TokenType.MdTag, "* ", 6, false, TagType.BulletedListItem),
            new(TokenType.Text, "два", 8)
        });
    }

    public static IEnumerable<TestCaseData> LinesWithEscapes()
    {
        yield return new TestCaseData(@"\", new List<Token>
        {
            new(TokenType.Escape, @"\", 0)
        });

        yield return new TestCaseData(@"\\", new List<Token>
        {
            new(TokenType.Escape, @"\", 0),
            new(TokenType.Escape, @"\", 1)
        });

        yield return new TestCaseData(@"\\\", new List<Token>
        {
            new(TokenType.Escape, @"\", 0),
            new(TokenType.Escape, @"\", 1),
            new(TokenType.Escape, @"\", 2),
        });
    }

    public static IEnumerable<TestCaseData> LinesWithUnderscores()
    {
        yield return new TestCaseData("_", new List<Token>
        {
            new(TokenType.MdTag, "_", 0, false, TagType.Italic)
        });

        yield return new TestCaseData("__", new List<Token>
        {
            new(TokenType.MdTag, "__", 0, false, TagType.Bold)
        });

        yield return new TestCaseData("___", new List<Token>
        {
            new(TokenType.MdTag, "__", 0, false, TagType.Bold),
            new(TokenType.MdTag, "_", 2, false, TagType.Italic)
        });

        yield return new TestCaseData("____", new List<Token>
        {
            new(TokenType.MdTag, "__", 0, false, TagType.Bold),
            new(TokenType.MdTag, "__", 2, false, TagType.Bold)
        });
    }

    public static IEnumerable<TestCaseData> LineWithMultiTokens()
    {
        yield return new TestCaseData("Bibo 234 _ # ", new List<Token>
        {
            new(TokenType.Text, "Bibo", 0),
            new(TokenType.WhiteSpace, " ", 4),
            new(TokenType.Number, "234", 5),
            new(TokenType.WhiteSpace, " ", 8),
            new(TokenType.MdTag, "_", 9, false, TagType.Italic),
            new(TokenType.WhiteSpace, " ", 10),
            new(TokenType.MdTag, "# ", 11, false, TagType.Header)
        });

        yield return new TestCaseData("__# _", new List<Token>
        {
            new(TokenType.MdTag, "__", 0, false, TagType.Bold),
            new(TokenType.MdTag, "# ", 2, false, TagType.Header),
            new(TokenType.MdTag, "_", 4, false, TagType.Italic)
        });

        yield return new TestCaseData("_2_3_", new List<Token>
        {
            new(TokenType.MdTag, "_", 0, false, TagType.Italic),
            new(TokenType.Number, "2", 1),
            new(TokenType.MdTag, "_", 2, false, TagType.Italic),
            new(TokenType.Number, "3",3),
            new(TokenType.MdTag, "_", 4, false, TagType.Italic)
        });

        yield return new TestCaseData(@"\# word", new List<Token>
        {
            new(TokenType.Escape, @"\", 0),
            new(TokenType.MdTag, "# ", 1, false, TagType.Header),
            new(TokenType.Text, "word", 3)
        });
    }
}