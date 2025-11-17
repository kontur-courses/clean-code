using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown;

[TestFixture]
public class MarkdownTests
{
    private static Object[] _tokenizePairTestCases = 
    {
        new Object[]
        {
            "_текст текст текст_",
            new List<Token>
            {
                new Token(0, 17, TokenType.Italic, 1)
            },
        },
        new Object[]
        {
            "__текст текст текст__",
            new List<Token>
            {
                new Token(0, 17, TokenType.Bold, 2)
            },
        },
    };
    
    [TestCaseSource(nameof(_tokenizePairTestCases))]
    public void TokenizeBasicPairTags(string rawText, List<Token> expectedTokens)
    {
        var parser = new MarkdownParser();
        parser.TokenizeText(rawText).Should().BeEquivalentTo(expectedTokens);
    }
    
    private static Object[] _wrapTokensTestCases = 
    {
        new Object[]
        {
            new List<Token>
            {
                new Token(0, 17, TokenType.Italic, 1)
            },
            new List<Token>
            {
                new Token(0, 17, TokenType.Italic, 1)
                    .SetTokenWrappers(new TokenWrappers("<em>", "</em>"))
            },
        },
        new Object[]
        {
            new List<Token>
            {
                new Token(0, 17, TokenType.Bold, 2)
            },
            new List<Token>
            {
                new Token(0, 17, TokenType.Bold, 2)
                    .SetTokenWrappers(new TokenWrappers("<strong>", "</strong>"))
            },
        },
        new Object[]
        {
            new List<Token>
            {
                new Token(0, 17, TokenType.Title, 2)
            },
            new List<Token>
            {
                new Token(0, 17, TokenType.Title, 2)
                    .SetTokenWrappers(new TokenWrappers("<h1>", "</h1>"))
            },
        },
    };
    
    [TestCaseSource(nameof(_wrapTokensTestCases))]
    public void WrapBasicTags(List<Token> tokens, List<Token> expected)
    {
        var parser = new MarkdownParser();
        parser.AddTokenWrappers(tokens).Should().BeEquivalentTo(expected);
    }
    
    [TestCase("_текст текст текст_", "<em>текст текст текст</em>")]
    [TestCase("__текст текст текст__", "<strong>текст текст текст</strong>")]
    [TestCase("# текст текст текст", "<h1>текст текст текст</h1>")]
    [TestCase("текст текст текст", "текст текст текст")]
    [TestCase("текст\n* текст\n* текст", "текст<ul><li>текст</li><li>текст</li></ul>")]
    public void ParseBasicTags(string rawText, string expectedText)
    {
        var parser = new MarkdownParser();
        parser.Render(rawText).Should().BeEquivalentTo(expectedText);
    }

    [TestCase(@"текст \текст тек\ст", @"текст \текст тек\ст")]
    [TestCase(@"\_текст текст \_текст", @"\_текст текст \_текст")]
    [TestCase(@"\\_текст текст\\_ текст", @"\\<em>текст текст\\</em> текст")]
    public void TestScreeningParsing(string rawText, string expectedText)
    {
        var parser = new MarkdownParser();
        parser.Render(rawText).Should().BeEquivalentTo(expectedText);
    }

    [TestCase("__текст _текст_ текст__", "<strong>текст <em>текст</em> текст</strong>")]
    [TestCase("_текст __текст__ текст_", "<em>текст __текст__ текст</em>")]
    [TestCase("текст текст_12_3", "текст текст_12_3")]
    [TestCase("_тек_ст те_кс_т тек_ст_", "<em>тек</em>ст те<em>кс</em>т тек<em>ст</em>")]
    [TestCase("текст т_екст тек_ст", "текст т_екст тек_ст")]
    [TestCase("текст __текст_ текст", "текст __текст_ текст")]
    [TestCase("текст_ текст_ текст", "текст_ текст_ текст")]
    [TestCase("текст _текст _текст", "текст _текст _текст")]
    [TestCase("__текст _текст__ _текст", "__текст _текст__ _текст")]
    [TestCase("____текст текст текст", "____текст текст текст")]
    [TestCase("____", "____")]
    [TestCase("# __текст _текст_ текст__", "<h1><strong>текст <em>текст</em> текст</strong></h1>")]
    [TestCase("текст\n* _текст_\n* __текст__", "текст<ul><li><em>текст</em></li><li><strong>текст</strong></li></ul>")]
    public void TestTagsInteraсtions(string rawText, string expectedText)
    {
        var parser = new MarkdownParser();
        parser.Render(rawText).Should().BeEquivalentTo(expectedText);
    }

    [TestCase(10001)]
    [TestCase(100001)]
    [TestCase(1000001)]
    //[TestCase(10000001)] // Этот тест работает около 6 секунд, раскомментировать только
    public void PerformanceTests(int stringLength)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < stringLength; i++)
        {
            sb.Append("__a");
        }
        sb.Append("__");
        var s = sb.ToString();
        var parser = new MarkdownParser();
        var res = parser.Render(s);
        Console.WriteLine(res);

    }
}