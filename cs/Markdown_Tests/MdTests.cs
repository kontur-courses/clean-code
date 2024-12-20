using FluentAssertions;
using Markdown;

namespace Markdown_Tests;

public class MdTests
{
    private IStringProcessor md;

    [SetUp]
    public void Setup()
    {
        md = new Md();
    }

    [Test]
    public void Render_ThrowsException_WhenArgumentIsNull()
    {
        var render = () => md.Render(null!);
        render.Should().Throw<ArgumentNullException>();
    }

    [TestCase("")]
    [TestCase("abc")]
    [TestCase("a b")]
    [TestCase("a\nb")]
    [Description("Не изменяет строку без тегов")]
    public void Render_ReturnsSameText_WhenWithoutTags(string mdString)
    {
        var htmlString = md.Render(mdString);
        htmlString.Should().Be(mdString);
    }

    [TestCaseSource(nameof(GetTextsWithValidPairTags), new object[] { "_", "em" })]
    [TestCaseSource(nameof(GetTextsWithInvalidPairTags), new object[] { "_", "em" })]
    [Description("Курсив")]
    public void Render_WorksWithItalicTextTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCaseSource(nameof(GetTextsWithValidPairTags), new object[] { "__", "strong" })]
    [TestCaseSource(nameof(GetTextsWithInvalidPairTags), new object[] { "__", "strong" })]
    [Description("Жирный текст")]
    public void Render_WorksWithBoldTextTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("# a", "<h1>a</h1>")]
    [TestCase("a\n# b", "a\n<h1>b</h1>")]
    [TestCase("# a\nb", "<h1>a</h1>\nb", Description = "В заголовок входят символы до конца строки")]
    [TestCase("a # b", "a # b", Description = "Заголовок должен быть в начале строки")]
    [Description("Заголовок")]
    public void Render_WorksWithHeaderTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("<a>", "<a href=\"a\">a</a>")]
    [TestCase("[a](b)", "<a href=\"b\">a</a>")]
    [Description("Ссылка")]
    public void Render_WorksWithLinkTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("![a](b)", "<img src=\"b\" alt=\"a\">")]
    [Description("Изображение")]
    public void Render_WorksWithImageTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("1. a\n2. b", "<ol><li>a</li>\n<li>b</li></ol>")]
    [TestCase("12345. a\n99999. b", "<ol><li>a</li>\n<li>b</li></ol>")]
    [TestCase("1. a\n2. b\nc", "<ol><li>a</li>\n<li>b</li></ol>\nc")]
    [TestCase("1. a\n2. b\nc\n1. d", "<ol><li>a</li>\n<li>b</li></ol>\nc<ol>\n<li>d</li></ol>")]
    [Description("Нумерованный список")]
    public void Render_WorksWithOrderedListTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("- a\n- b", "<ul><li>a</li>\n<li>b</li></ul>")]
    [TestCase("- a\n- b\nc", "<ul><li>a</li>\n<li>b</li></ul>\nc")]
    [TestCase("- a\n- b\nc\n- d", "<ul><li>a</li>\n<li>b</li></ul>\nc<ul>\n<li>d</li></ul>")]
    [Description("Ненумерованный список")]
    public void Render_WorksWithUnorderedListTag(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase(@"\a", @"\a")]
    [TestCase(@"\_a_", @"_a_")]
    [TestCase(@"\__a__", @"__a__")]
    [TestCase(@"\# a", @"# a")]
    [TestCase(@"\<a>", @"<a>")]
    [TestCase(@"\[a](b)", @"[a](b)")]
    [TestCase(@"\- a", @"- a")]
    [TestCase(@"\\_a_", @"\<em>a</em>")]
    [TestCase(@"\__a_", @"_<em>a</em>")]
    [TestCase(@"\![a](b)", @"!<a href=""b"">a</a>")]
    [Description("Экранирование")]
    public void Render_IgnoresEscapedTags(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [Test]
    [Description("Часть двойного подчеркивания не считается парой к одиночному")]
    public void Render_IgnoresInvalidPairTags()
    {
        var mdString = "__a_";
        var htmlString = md.Render(mdString);
        htmlString.Should().Be(mdString);
    }

    [TestCase("# a _b_", "<h1>a <em>b</em></h1>")]
    [TestCase("# a __b__", "<h1>a <strong>b</strong></h1>")]
    [Description("Курсив и жирный текст внутри заголовка работают")]
    public void Render_ItalicAndBoldTextTagsWorkInsideHeader(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>")]
    [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>")]
    [Description("Курсив внутри жирного текста работает")]
    public void Render_ItalicTextTagsWorkInsideBoldText(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("_a __b__ c_", "<em>a __b__ c</em>")]
    [TestCase("_a__b__c_", "<em>a__b__c</em>")]
    [Description("Жирный текст внутри курсива не работает")]
    public void Render_BoldTextTagsDoNotWorkInsideItalicText(string mdString, string expectedHtmlString)
    {
        var actualHtmlString = md.Render(mdString);
        actualHtmlString.Should().Be(expectedHtmlString);
    }

    [TestCase("a_b1_2")]
    [TestCase("a__b1__2")]
    [TestCase("a_b1___2")]
    [Description("Окруженные цифрами подчеркивания не считаются тегами")]
    public void Render_IgnoresUnderscoresSurroundedByNumbers(string mdString)
    {
        var htmlString = md.Render(mdString);
        htmlString.Should().Be(mdString);
    }

    [TestCase("__a _b c__ d_")]
    [TestCase("___a___")]
    [Description("В случае пересечения областей парных тегов, они не считаются тегами")]
    public void Render_PairTagsDoNotWork_WhenIntersected(string mdString)
    {
        var htmlString = md.Render(mdString);
        htmlString.Should().Be(mdString);
    }

    [Test]
    [Description("Работает за линейное время")]
    public void Render_PerformanceTest()
    {
        var mdString = "__a__ _b_\n# c \\__d__ <e> [f](g) ![h](k)\n1. l\n2. m\n- n\n o";
        var minRepetitionsCount = 64;
        var maxRepetitionsCount = 512;
        var step = 2;
        var error = 1;

        var benchmark = new Benchmark();
        var previousTime = 0L;

        for (var count = minRepetitionsCount; count <= maxRepetitionsCount; count *= step)
        {
            var arg = string.Concat(Enumerable.Repeat(mdString, count));
            var time = benchmark.MeasureMilliseconds(() => md.Render(arg), 100);

            if (previousTime != 0)
            {
                time.Should().BeLessThan((step + error) * previousTime);
            }

            previousTime = time;
        }
    }

    private static IEnumerable<TestCaseData> GetTextsWithValidPairTags(string mdTag, string htmlTag)
    {
        yield return new TestCaseData($"{mdTag}a{mdTag}", $"<{htmlTag}>a</{htmlTag}>");
        yield return new TestCaseData($"{mdTag}a b{mdTag}", $"<{htmlTag}>a b</{htmlTag}>");
        yield return new TestCaseData($"{mdTag}a\nb{mdTag}", $"<{htmlTag}>a\nb</{htmlTag}>");
        yield return new TestCaseData($"a {mdTag}b{mdTag} c", $"a <{htmlTag}>b</{htmlTag}> c");
        yield return new TestCaseData($"a{mdTag}b{mdTag}c", $"a<{htmlTag}>b</{htmlTag}>c");
        yield return new TestCaseData($"{mdTag}a{mdTag}b", $"<{htmlTag}>a</{htmlTag}>b");
        yield return new TestCaseData($"a{mdTag}b{mdTag}", $"a<{htmlTag}>b</{htmlTag}>");
        yield return new TestCaseData($"{mdTag}a{mdTag}b{mdTag}c{mdTag}", $"<{htmlTag}>a</{htmlTag}>b<{htmlTag}>c</{htmlTag}>");
    }

    private static IEnumerable<TestCaseData> GetTextsWithInvalidPairTags(string mdTag, string htmlTag)
    {
        yield return new TestCaseData($"{mdTag}a", $"{mdTag}a")
            .SetDescription("Только открывающий тег не считается тегом");
        yield return new TestCaseData($"a{mdTag}", $"a{mdTag}")
            .SetDescription("Только закрывающий тег не считается тегом");
        yield return new TestCaseData($"{mdTag}{mdTag}", $"{mdTag}{mdTag}")
            .SetDescription("Если между парными тегами пустая строка, они не считаются тегами");
        yield return new TestCaseData($"{mdTag}a {mdTag}b", $"{mdTag}a {mdTag}b")
            .SetDescription("Закрывающий тег не может иметь пробел перед ним");
        yield return new TestCaseData($"a{mdTag} b{mdTag}", $"a{mdTag} b{mdTag}")
            .SetDescription("Открывающий тег не может иметь пробел после него");
        yield return new TestCaseData($"a{mdTag}b c{mdTag}d", $"a{mdTag}b c{mdTag}d")
            .SetDescription("Теги в разных словах не считаются парой тегов");
        yield return new TestCaseData($"{mdTag}a {mdTag}b{mdTag}", $"{mdTag}a <{htmlTag}>b</{htmlTag}>")
            .SetDescription("Парой к закрывающему тегу считается тот открывающий тег, который ближе");
        yield return new TestCaseData($"{mdTag}a{mdTag} b{mdTag}", $"<{htmlTag}>a</{htmlTag}> b{mdTag}")
            .SetDescription("Парой к открывающему тегу считается тот закрывающий тег, который ближе");
        yield return new TestCaseData($"{mdTag}a{mdTag}b{mdTag}", $"<{htmlTag}>a</{htmlTag}>b{mdTag}")
            .SetDescription("Если в одном слове нечетное число тегов, парой считаются те, что левее");
    }
}
