using System.Diagnostics;
using System.Text;
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

    /* Italic */
    [TestCase("_Hello, World!_", "<em>Hello, World!</em>", TestName = "Italic is accepted")]
    /* Bold */
    [TestCase("__Hello, World!__", "<strong>Hello, World!</strong>", TestName = "Bold is accepted")]
    /*Header*/
    [TestCase("# Hello, World!", "<h1>Hello, World!</h1>", TestName = "Header is accepted")]
    /*Escape*/
    [TestCase(@"\_Hello\_", "_Hello_", TestName = "Escape italic")]
    [TestCase(@"\_\_Hello\_\_", "__Hello__", TestName = "Escape bold")]
    [TestCase(@"\Hello\", @"\Hello\", TestName = "Escaping character does not disappear if there is nothing to escape")]
    [TestCase(@"\\_Hello_", @"\<em>Hello</em>", TestName = "Escaping character can be escaped")]
    [TestCase(@"_Hello,\ World!_", @"<em>Hello, World!</em>", TestName = "Space character can be escaped")]
    /*Tag interaction*/
    [TestCase("__Kill _me_ please__", "<strong>Kill <em>me</em> please</strong>",
        TestName = "Inside a double selection, a single selection also works")]
    [TestCase("_Kill __me__ please_", "<em>Kill __me__ please</em>",
        TestName = "Inside single selection double selection not working")]
    [TestCase("Hello_2B_and_9S", "Hello_2B_and_9S",
        TestName = "Single underscores inside numeric text are not considered highlights and must remain underscores")]
    [TestCase("Hello__2B__and__9S", "Hello__2B__and__9S",
        TestName = "Double underscores inside numeric text are not considered highlights and must remain underscores")]
    [TestCase("_Hell_o", "<em>Hell</em>o", TestName = "Single underscore works at the beginning of a word")]
    [TestCase("__Hell__o", "<strong>Hell</strong>o", TestName = "Double underscore works at the beginning of a word")]
    [TestCase("pi_gg_ie", "pi<em>gg</em>ie", TestName = "Single underscore works in the middle of a word")]
    [TestCase("pi__gg__ie", "pi<strong>gg</strong>ie", TestName = "Double underscore works in the middle of a word")]
    [TestCase("cl_ass_", "cl<em>ass</em>", TestName = "Single underscore works at the end of a word")]
    [TestCase("cl__ass__", "cl<strong>ass</strong>", TestName = "Double underscore works at the end of a word")]
    [TestCase("hell_o cl_ass", TestName = "Italic selection does not work in different words")]
    [TestCase("hell__o cl__ass", TestName = "Bold selection does not work in different words")]
    [TestCase("__Hello_", TestName = "Double-Single: Unpaired characters within the same paragraph are not considered")]
    [TestCase("_Hello__", TestName = "Single-Double: Unpaired characters within the same paragraph are not considered")]
    [TestCase("_ Hello_", TestName = "Italic opening character must be followed by a non-whitespace character")]
    [TestCase("_Hello _", TestName = "The closing italic character must be preceded by a non-whitespace character")]
    [TestCase("__ Hello__", TestName = "Bold opening character must be followed by a non-whitespace character")]
    [TestCase("__Hello __", TestName = "The closing bold character must be preceded by a non-whitespace character")]
    [TestCase("C_r__o_s__s",
        TestName =
            "Single-Double: On the intersection of double and single underscores, none of them are considered highlighted")]
    [TestCase("C__r_o__s_s",
        TestName =
            "Double-Single: On the intersection of double and single underscores, none of them are considered highlighted")]
    [TestCase("__", TestName = "On an empty line inside single underscores, then they remain underscores characters")]
    [TestCase("____", TestName = "On an empty line inside double underscores, then they remain underscores characters")]
    [TestCase("# Header with _italic_ characters", "<h1>Header with <em>italic</em> characters</h1>",
        TestName = "Header with italic characters")]
    [TestCase("# Header with __bold__ characters", "<h1>Header with <strong>bold</strong> characters</h1>",
        TestName = "Header with bold characters")]
    public void Render_Format_OnTest(string input, string? expected = null)
    {
        md.Render(input).Should().Be(expected ?? input);
    }
}