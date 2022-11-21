using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

public class Md_Render_Should
{
    private Md md;

    [SetUp]
    public void Setup()
    {
        md = new Md();
    }
    
    [TestCaseSource(nameof(ItalicTests))]
    [TestCaseSource(nameof(BoldTests))]
    [TestCaseSource(nameof(HeaderTests))]
    [TestCaseSource(nameof(EscapeTests))]
    [TestCaseSource(nameof(HierarchyTests))]
    [TestCaseSource(nameof(InWordsInteractionTests))]
    public void Render_Format_OnTest(string input, string? expected = null)
    {
        md.Render(input).Should().Be(expected ?? input);
    }
    
    static TestCaseData[] ItalicTests =
    {
        new TestCaseData("_Hello, World!_", "<em>Hello, World!</em>").SetName("Italic is accepted"),
        new TestCaseData("_ Hello_").SetName("Italic opening character must be followed by a non-whitespace character"),
        new TestCaseData("_Hello _").SetName("The closing italic character must be preceded by a non-whitespace character"),
        new TestCaseData("__").SetName("On an empty line inside single underscores, then they remain underscores characters")
    };
        
    static TestCaseData[] BoldTests =
    {
        new TestCaseData("__Hello, World!__", "<strong>Hello, World!</strong>").SetName("Bold is accepted"),
        new TestCaseData("__ Hello__").SetName("Bold opening character must be followed by a non-whitespace character"),
        new TestCaseData("__Hello __").SetName("The closing bold character must be preceded by a non-whitespace character"),
        new TestCaseData("____").SetName("On an empty line inside double underscores, then they remain underscores characters")
    };
        
    static TestCaseData[] HeaderTests =
    {
        new TestCaseData("# Hello, World!", "<h1>Hello, World!</h1>").SetName("Header is accepted"),
        new TestCaseData("# Header with _italic_ characters", "<h1>Header with <em>italic</em> characters</h1>").SetName("Header with italic characters"),
        new TestCaseData("# Header with __bold__ characters", "<h1>Header with <strong>bold</strong> characters</h1>").SetName("Header with bold characters")
    };
        
    static TestCaseData[] EscapeTests =
    {
        new TestCaseData(@"\_Hello\_", "_Hello_").SetName("Escape italic"),
        new TestCaseData(@"\_\_Hello\_\_", "__Hello__").SetName("Escape bold"),
        new TestCaseData(@"\Hello\", @"\Hello\").SetName("Escaping character does not disappear if there is nothing to escape"),
        new TestCaseData(@"\\_Hello_", @"\<em>Hello</em>").SetName("Escaping character can be escaped"),
        new TestCaseData(@"_Hello,\ World!_", @"<em>Hello, World!</em>").SetName("Space character can be escaped"),
    };
        
    static TestCaseData[] HierarchyTests =
    {
        new TestCaseData("__Kill _me_ please__", "<strong>Kill <em>me</em> please</strong>").SetName("Inside a double selection, a single selection also works"),
        new TestCaseData("_Kill __me__ please_", "<em>Kill __me__ please</em>").SetName("Inside single selection double selection not working"),
        new TestCaseData("C_r__o_s__s").SetName("Single-Double: On the intersection of double and single underscores, none of them are considered highlighted"),
        new TestCaseData("C__r_o__s_s").SetName("Double-Single: On the intersection of double and single underscores, none of them are considered highlighted"),
        new TestCaseData("__Hello_").SetName("Double-Single: Unpaired characters within the same paragraph are not considered"),
        new TestCaseData("_Hello__").SetName("Single-Double: Unpaired characters within the same paragraph are not considered")
    };
        
    static TestCaseData[] InWordsInteractionTests =
    {
        new TestCaseData("Hello_2B_and_9S", "Hello_2B_and_9S").SetName("Single underscores inside numeric text are not considered highlights and must remain underscores"),
        new TestCaseData("Hello__2B__and__9S", "Hello__2B__and__9S").SetName("Double underscores inside numeric text are not considered highlights and must remain underscores"),
        new TestCaseData("_Hell_o", "<em>Hell</em>o").SetName("Single underscore works at the beginning of a word"),
        new TestCaseData("__Hell__o", "<strong>Hell</strong>o").SetName("Double underscore works at the beginning of a word"),
        new TestCaseData("pi_gg_ie", "pi<em>gg</em>ie").SetName("Single underscore works in the middle of a word"),
        new TestCaseData("pi__gg__ie", "pi<strong>gg</strong>ie").SetName("Double underscore works in the middle of a word"),
        new TestCaseData("cl_ass_", "cl<em>ass</em>").SetName("Single underscore works at the end of a word"),
        new TestCaseData("cl__ass__", "cl<strong>ass</strong>").SetName("Double underscore works at the end of a word"),
        new TestCaseData("hell_o cl_ass").SetName("Italic selection does not work in different words"),
        new TestCaseData("hell__o cl__ass").SetName("Bold selection does not work in different words")
    };
}